﻿namespace ProcessingTools.Bio.Taxonomy.Processors.Parsers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml;

    using Comparers;
    using Contracts.Parsers;
    using Models.Parsers;

    using ProcessingTools.Bio.Taxonomy.Constants;
    using ProcessingTools.Bio.Taxonomy.Extensions;
    using ProcessingTools.Bio.Taxonomy.Types;
    using ProcessingTools.Contracts;
    using ProcessingTools.Contracts.Types;
    using ProcessingTools.Extensions;
    using ProcessingTools.Xml.Extensions;

    public class Expander : IExpander
    {
        private readonly ILogger logger;

        private readonly Expression<Func<ITaxonNamePart, bool>> partIsAbbreviatedAndNotResolved = p => p.IsAbbreviated && !p.IsResolved;
        private readonly Expression<Func<ITaxonNamePart, bool>> partIsResolvedOrNotAbbreviated = p => p.IsResolved || !p.IsAbbreviated;
        private readonly Expression<Func<ITaxonNamePart, bool>> partIsWithMeaningfullRank = p => p.Rank != SpeciesPartType.Undefined;
        private readonly Expression<Func<ITaxonNamePart, bool>> matchResolvedGenera = p => (p.Rank == SpeciesPartType.Genus) && (p.IsResolved || !p.IsAbbreviated);
        private readonly Expression<Func<ITaxonNamePart, bool>> matchUnResolvedGenera = p => (p.Rank == SpeciesPartType.Genus) && (p.IsAbbreviated && !p.IsResolved);

        public Expander(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<object> Parse(XmlNode context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            await Task.Run(() =>
            {
                this.EnsureFullNameAttributeToTaxonNamePartElements(context);

                this.AddIdAndPositionAttributesToTaxonNameElements(context);

                try
                {

                    var taxonNames = this.GetContextTaxonVectorModel(context);

                    this.PrintContextVectorModel(taxonNames);

                    this.UniqueGenusMatchExpand(taxonNames);
                    this.StableExpand(taxonNames);
                    this.ImportModifiedItems(context, taxonNames);

                    this.StableExpand(context);
                    this.ForceExactSpeciesMatchExpand(context);
                }
                catch (Exception e)
                {
                    this.logger?.Log(e, string.Empty);
                }

                this.RemoveIdAndPositionAttributesToTaxonNameElements(context);
            });

            return true;
        }

        private void EnsureFullNameAttributeToTaxonNamePartElements(XmlNode context)
        {
            context.SelectNodes($"{XmlInternalSchemaConstants.SelectTaxonNamePartsOfLowerTaxonNamesXPath}[not(@{XmlInternalSchemaConstants.FullNameAttributeName})]")
                .Cast<XmlElement>()
                .AsParallel()
                .ForAll(n =>
                {
                    var rank = n.GetAttribute(XmlInternalSchemaConstants.TypeAttributeName).ToSpeciesPartType();
                    if (rank == SpeciesPartType.Undefined)
                    {
                        if (n.HasAttribute(XmlInternalSchemaConstants.FullNameAttributeName))
                        {
                            n.RemoveAttribute(XmlInternalSchemaConstants.FullNameAttributeName);
                        }
                    }
                    else
                    {
                        if (!n.HasAttribute(XmlInternalSchemaConstants.FullNameAttributeName))
                        {
                            string content = n.InnerText;
                            if (string.IsNullOrWhiteSpace(content) || content.Contains("."))
                            {
                                n.SetAttribute(XmlInternalSchemaConstants.FullNameAttributeName, string.Empty);
                            }
                            else
                            {
                                n.SetAttribute(XmlInternalSchemaConstants.FullNameAttributeName, content);
                            }
                        }
                    }
                });
        }

        private void AddIdAndPositionAttributesToTaxonNameElements(XmlNode context)
        {
            long counter = 1L;

            {
                var taxonNameElements = context.SelectNodes(XmlInternalSchemaConstants.SelectLowerTaxonNamesXPath)
                    .Cast<XmlElement>();

                foreach (var taxonNameElement in taxonNameElements)
                {
                    taxonNameElement.SetAttribute(
                        XmlInternalSchemaConstants.IdAttributeName,
                        XmlInternalSchemaConstants.TaxonNameIdPrefix + counter);
                    taxonNameElement.SetAttribute(
                        XmlInternalSchemaConstants.PositionAttributeName,
                        counter.ToString());
                    ++counter;
                }
            }

            {
                var taxonNamePartElements = context.SelectNodes(XmlInternalSchemaConstants.SelectTaxonNamePartsOfLowerTaxonNamesXPath)
                    .Cast<XmlElement>();

                foreach (var taxonNamePartElement in taxonNamePartElements)
                {
                    taxonNamePartElement.SetAttribute(
                        XmlInternalSchemaConstants.IdAttributeName,
                        XmlInternalSchemaConstants.TaxonNamePartIdPrefix + counter);
                    ++counter;
                }
            }
        }

        private void RemoveIdAndPositionAttributesToTaxonNameElements(XmlNode context)
        {
            context.SelectNodes(XmlInternalSchemaConstants.SelectLowerTaxonNamesXPath)
                .Cast<XmlElement>()
                .AsParallel()
                .ForAll(n =>
                {
                    n.RemoveAttribute(XmlInternalSchemaConstants.IdAttributeName);
                    n.RemoveAttribute(XmlInternalSchemaConstants.PositionAttributeName);
                });

            context.SelectNodes(XmlInternalSchemaConstants.SelectTaxonNamePartsOfLowerTaxonNamesXPath)
                .Cast<XmlElement>()
                .AsParallel()
                .ForAll(n =>
                {
                    n.RemoveAttribute(XmlInternalSchemaConstants.IdAttributeName);
                });
        }

        private void StableExpand(XmlNode context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // In this method it is supposed that the subspecies name is not shortened
            this.PrintMethodMessage(nameof(this.StableExpand));

            var shortTaxaListUnique = this.GetListOfShortenedTaxa(context);
            var nonShortTaxaListUnique = this.GetListOfNonShortenedTaxa(context);

            var speciesList = nonShortTaxaListUnique
                .Select(t => new Species(t))
                .ToList();

            string xml = context.InnerXml;

            foreach (string shortTaxon in shortTaxaListUnique)
            {
                string text = Regex.Replace(shortTaxon, " xmlns:\\w+=\".*?\"", string.Empty);
                string replace = text;

                var sp = new Species(shortTaxon);
                this.PrintNextShortened(sp);

                foreach (var sp1 in speciesList)
                {
                    if (string.Compare(sp.SubspeciesName, sp1.SubspeciesName, true) != 0)
                    {
                        continue;
                    }

                    var matchGenus = Regex.Match(sp1.GenusName, sp.GenusPattern);
                    var matchSubgenus = Regex.Match(sp1.SubgenusName, sp.SubgenusPattern);
                    var matchSpecies = Regex.Match(sp1.SpeciesName, sp.SpeciesPattern);

                    if (string.IsNullOrWhiteSpace(sp.SubgenusName))
                    {
                        if (matchGenus.Success && matchSpecies.Success)
                        {
                            if (string.IsNullOrWhiteSpace(sp1.SubgenusName))
                            {
                                this.PrintSubstitutionMessage(sp, sp1);
                                replace = replace
                                    .RegexReplace("(?<=type=\"genus\"[^>]+full-name=\")(?=\")", sp1.GenusName)
                                    .RegexReplace("(?<=type=\"species\"[^>]+full-name=\")(?=\")", sp1.SpeciesName);
                            }
                            else
                            {
                                this.PrintSubstitutionMessageFail(sp, sp1);
                            }
                        }
                    }
                    else
                    {
                        if (matchGenus.Success && matchSubgenus.Success && matchSpecies.Success)
                        {
                            this.PrintSubstitutionMessage(sp, sp1);
                            replace = replace
                                .RegexReplace("(?<=type=\"genus\"[^>]+full-name=\")(?=\")", sp1.GenusName)
                                .RegexReplace("(?<=type=\"subgenus\"[^>]+full-name=\")(?=\")", sp1.SubgenusName)
                                .RegexReplace("(?<=type=\"species\"[^>]+full-name=\")(?=\")", sp1.SpeciesName);
                        }
                    }
                }

                xml = Regex.Replace(xml, Regex.Escape(text), replace);
            }

            context.InnerXml = xml;
        }

        private void StableExpand(IQueryable<ITaxonName> taxonNames)
        {
            var abbreviatedTaxonNames = taxonNames.Where(tn => tn.Parts.Where(this.partIsWithMeaningfullRank).Any(this.partIsAbbreviatedAndNotResolved));
            var expandedTaxonNames = taxonNames.Where(tn => tn.Parts.All(this.partIsResolvedOrNotAbbreviated))
                .Distinct(new TaxonNameContentEqualityComparer())
                .ToList();

            var messages = new ConcurrentQueue<string>();

            abbreviatedTaxonNames.AsParallel()
                .ForAll(abbreviatedTaxonName =>
                {
                    var messageBag = new StringBuilder();
                    messageBag.Append(abbreviatedTaxonName);
                    messageBag.AppendLine();

                    var query = expandedTaxonNames.AsQueryable();

                    var meaningfullTaxonNameParts = abbreviatedTaxonName.Parts.Where(this.partIsWithMeaningfullRank);

                    // Builds the query
                    foreach (var taxonNamePart in meaningfullTaxonNameParts)
                    {
                        query = query.Where(e => e.Parts.Any(taxonNamePart.MatchExpression));
                    }

                    // Executes the query and gets matches
                    var matches = query.Select(e => new MinimalTaxonName
                    {
                        Parts = e.Parts.Select(p => new MinimalTaxonNamePart
                        {
                            Name = p.Name,
                            FullName = p.FullName,
                            Rank = p.Rank
                        })
                    })
                    .Distinct()
                    .ToArray();

                    if (matches.Length < 1)
                    {
                        messageBag.Append("\tNo matches.");
                        messageBag.AppendLine();
                    }
                    else
                    {
                        foreach (var taxonNamePart in meaningfullTaxonNameParts.Where(this.partIsAbbreviatedAndNotResolved))
                        {
                            var matchedFullNames = matches.SelectMany(t => t.Parts.Where(p => p.Rank == taxonNamePart.Rank))
                                .Select(p => p.FullName)
                                .Distinct()
                                .ToArray();

                            if (matchedFullNames.Length < 1)
                            {
                                messageBag.AppendFormat("\tError: taxon-name-part of rank {0} does not have valid matches:", taxonNamePart.Rank);
                                messageBag.AppendLine();
                            }
                            else if (matchedFullNames.Length > 1)
                            {
                                messageBag.AppendFormat("\tError: Multiple matches ({0}):", taxonNamePart.Rank);
                                messageBag.AppendLine();
                            }
                            else
                            {
                                var name = matchedFullNames[0];
                                taxonNamePart.FullName = name;
                                taxonNamePart.IsModified = true;

                                messageBag.AppendFormat("\tSubstitution ({0}):", taxonNamePart.Rank);
                                messageBag.AppendLine();
                            }

                            foreach (var match in matches)
                            {
                                messageBag.AppendFormat("\t\t{0}", match);
                                messageBag.AppendLine();
                            }
                        }
                    }

                    messageBag.AppendLine();

                    messages.Enqueue(messageBag.ToString());
                });

            foreach (var message in messages)
            {
                this.logger?.Log(message);
            }
        }

        private void UniqueGenusMatchExpand(IQueryable<ITaxonName> taxonNames)
        {
            var messages = new ConcurrentQueue<string>();

            var query = taxonNames.SelectMany(t => t.Parts.Where(this.matchResolvedGenera).Select(p => p.FullName));
            var genera = new HashSet<string>(query.ToArray());

            var abbreviatedGenera = taxonNames.SelectMany(t => t.Parts.Where(this.matchUnResolvedGenera));
            abbreviatedGenera.AsParallel()
                .ForAll(abbreviatedGenus =>
                {
                    var matches = genera.Where(g => g.IndexOf(abbreviatedGenus.Pattern) == 0).ToArray();
                    if (matches.Length == 1)
                    {
                        {
                            var messageBag = new StringBuilder();
                            messageBag.AppendFormat("\tSubstitution ({0}):", abbreviatedGenus);
                            messageBag.AppendLine();
                            foreach (var match in matches)
                            {
                                messageBag.AppendFormat("\t\t{0}", match);
                                messageBag.AppendLine();
                            }

                            messages.Enqueue(messageBag.ToString());
                        }

                        abbreviatedGenus.FullName = matches[0];
                        abbreviatedGenus.IsModified = true;
                    }
                });

            foreach (var message in messages)
            {
                this.logger?.Log(message);
            }
        }

        private IQueryable<ITaxonName> GetContextTaxonVectorModel(XmlNode context)
        {
            var taxa = context.SelectNodes(XmlInternalSchemaConstants.SelectLowerTaxonNamesXPath)
                .Cast<XmlNode>()
                .Select(t => new TaxonName(t))
                .ToArray();

            return taxa.AsQueryable();
        }

        private void ImportModifiedItems(XmlNode context, IQueryable<ITaxonName> taxonNames)
        {
            var modifiedTaxonNameParts = taxonNames
                .Where(tn => tn.Parts.Any(p => p.IsModified))
                .SelectMany(tn => tn.Parts.Where(p => p.IsModified))
                .ToArray();

            modifiedTaxonNameParts.AsParallel()
                .ForAll(p =>
                {
                    var xpath = $".//{XmlInternalSchemaConstants.TaxonNamePartElementName}[@{XmlInternalSchemaConstants.IdAttributeName}='{p.Id}']";

                    var taxonNamePartElement = context.SelectSingleNode(xpath) as XmlElement;
                    taxonNamePartElement.SetAttribute(XmlInternalSchemaConstants.FullNameAttributeName, p.FullName);
                });
        }

        private void ForceExactSpeciesMatchExpand(XmlNode context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            string nodeListOfSpeciesInShortenedTaxaNameXPath = $"{XmlInternalSchemaConstants.SelectLowerTaxonNamesXPath}[normalize-space({XmlInternalSchemaConstants.TaxonNamePartOfTypeSpeciesXPath})!=''][normalize-space({XmlInternalSchemaConstants.TaxonNamePartOfTypeGenusXPath})=''][normalize-space({XmlInternalSchemaConstants.TaxonNamePartOfTypeGenusXPath}/@full-name)='']/{XmlInternalSchemaConstants.TaxonNamePartOfTypeSpeciesXPath}";

            var speciesUniq = context.SelectNodes(nodeListOfSpeciesInShortenedTaxaNameXPath)
                .Cast<XmlNode>()
                .Select(n => n.InnerText)
                .Distinct()
                .ToList();

            speciesUniq.ForEach(s => this.logger?.Log(s));

            IDictionary<string, string[]> speciesGenusPairs = new Dictionary<string, string[]>();

            foreach (string species in speciesUniq)
            {
                var genera = context.SelectNodes($"{XmlInternalSchemaConstants.SelectLowerTaxonNamesXPath}[normalize-space({XmlInternalSchemaConstants.TaxonNamePartOfTypeSpeciesXPath})='{species}'][normalize-space({XmlInternalSchemaConstants.TaxonNamePartOfTypeGenusXPath})!='' or normalize-space({XmlInternalSchemaConstants.TaxonNamePartOfTypeGenusXPath}/@full-name)!='']/{XmlInternalSchemaConstants.TaxonNamePartOfTypeGenusXPath}")
                    .Cast<XmlElement>()
                    .Select(g =>
                    {
                        if ((string.IsNullOrWhiteSpace(g.InnerText) || g.InnerText.Contains('.')) && g.Attributes[XmlInternalSchemaConstants.FullNameAttributeName] != null)
                        {
                            return g.Attributes[XmlInternalSchemaConstants.FullNameAttributeName].InnerText;
                        }
                        else
                        {
                            return g.InnerText;
                        }
                    })
                    .Distinct()
                    .ToList();

                speciesGenusPairs.Add(species, genera.ToArray());
            }

            foreach (string species in speciesGenusPairs.Keys)
            {
                this.logger?.Log(species);

                switch (speciesGenusPairs[species].Length)
                {
                    case 0:
                        this.logger?.Log(LogType.Warning, "No matches.");
                        break;

                    case 1:
                        string genus = speciesGenusPairs[species].FirstOrDefault();
                        this.logger?.Log(genus);

                        context.SelectNodes($"{XmlInternalSchemaConstants.SelectLowerTaxonNamesXPath}[normalize-space({XmlInternalSchemaConstants.TaxonNamePartOfTypeSpeciesXPath})='{species}'][normalize-space({XmlInternalSchemaConstants.TaxonNamePartOfTypeGenusXPath})=''][normalize-space({XmlInternalSchemaConstants.TaxonNamePartOfTypeGenusXPath}/@full-name)='']/{XmlInternalSchemaConstants.TaxonNamePartOfTypeGenusXPath}")
                            .Cast<XmlElement>()
                            .AsParallel()
                            .ForAll(t =>
                            {
                                var fullNameAttribute = t.Attributes[XmlInternalSchemaConstants.FullNameAttributeName];
                                if (fullNameAttribute == null)
                                {
                                    t.SetAttribute(XmlInternalSchemaConstants.FullNameAttributeName, genus);
                                }
                                else
                                {
                                    fullNameAttribute.InnerText = genus;
                                }
                            });

                        break;

                    default:
                        this.logger?.Log(LogType.Warning, "Multiple matches:");
                        speciesGenusPairs[species].ToList().ForEach(g => this.logger?.Log("--> {0}", g));
                        break;
                }

                this.logger?.Log();
            }
        }

        private void PrintContextVectorModel(IQueryable<ITaxonName> taxonNames)
        {
            this.logger?.Log();

            foreach (var taxonName in taxonNames.OrderBy(t => t.Position))
            {
                this.logger?.Log(taxonName);
            }

            this.logger?.Log();
        }

        private void PrintMethodMessage(string name)
        {
            this.logger?.Log("\n\n#\n##\n### {0} will be executed...\n##\n#\n", name);
        }

        private void PrintNextShortened(Species sp)
        {
            this.logger?.Log("\nNext shortened taxon:\t{0}", sp.ToString());
        }

        private void PrintSubstitutionMessage(Species original, Species substitution)
        {
            this.logger?.Log("\tSubstitution:\t{0}\t-->\t{1}", original.ToString(), substitution.ToString());
        }

        private void PrintSubstitutionMessageFail(Species original, Species substitution)
        {
            this.logger?.Log("\tFailed Subst:\t{0}\t<->\t{1}", original.ToString(), substitution.ToString());
        }

        private IEnumerable<string> GetListOfNonShortenedTaxa(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var document = node.OwnerDocument();

            string xpath = $"{XmlInternalSchemaConstants.SelectLowerTaxonNamesXPath}[not(tn-part[@full-name=''])][{XmlInternalSchemaConstants.TaxonNamePartOfTypeGenusXPath}]";
            var result = node.SelectNodes(xpath)
                .Cast<XmlNode>()
                .Select(currentNode =>
                {
                    XmlElement taxonNameElement = document.CreateElement(XmlInternalSchemaConstants.TaxonNameElementName);
                    foreach (XmlNode innerNode in currentNode.SelectNodes(".//*"))
                    {
                        XmlElement taxonNamePartElement = document.CreateElement(XmlInternalSchemaConstants.TaxonNamePartElementName);

                        // Copy only *type* attributes
                        foreach (XmlAttribute attribute in innerNode.Attributes)
                        {
                            if (attribute.Name.Contains(XmlInternalSchemaConstants.TypeAttributeName))
                            {
                                XmlAttribute typeAttribute = document.CreateAttribute(attribute.Name);
                                typeAttribute.InnerText = attribute.InnerText;
                                taxonNamePartElement.Attributes.Append(typeAttribute);
                            }
                        }

                        // Gets the value of the @full-name attribute if present or the content of the node
                        var fullNameAttribute = innerNode.Attributes[XmlInternalSchemaConstants.FullNameAttributeName];
                        if (fullNameAttribute != null && !string.IsNullOrWhiteSpace(fullNameAttribute.InnerText))
                        {
                            taxonNamePartElement.InnerText = fullNameAttribute.InnerText;
                        }
                        else
                        {
                            taxonNamePartElement.InnerText = innerNode.InnerText;
                        }

                        taxonNameElement.AppendChild(taxonNamePartElement);
                    }

                    return taxonNameElement;
                })
                .ToList<XmlNode>()
                .GetStringListOfUniqueXmlNodes();

            return new HashSet<string>(result);
        }

        public IEnumerable<string> GetListOfShortenedTaxa(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            string xpath = $"{XmlInternalSchemaConstants.SelectLowerTaxonNamesXPath}[tn-part[@full-name[normalize-space(.)='']][normalize-space(.)!='']][{XmlInternalSchemaConstants.TaxonNamePartOfTypeGenusXPath}][normalize-space({XmlInternalSchemaConstants.TaxonNamePartOfTypeSpeciesXPath})!='']";
            var result = node.GetStringListOfUniqueXmlNodes(xpath);

            return new HashSet<string>(result);
        }
    }
}