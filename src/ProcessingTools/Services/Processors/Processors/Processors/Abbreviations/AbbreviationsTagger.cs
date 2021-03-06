﻿namespace ProcessingTools.Processors.Processors.Abbreviations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;
    using ProcessingTools.Common.Extensions;
    using ProcessingTools.Contracts;
    using ProcessingTools.Harvesters.Contracts.Harvesters.Abbreviations;
    using ProcessingTools.Processors.Contracts.Models.Abbreviations;
    using ProcessingTools.Processors.Contracts.Processors.Abbreviations;
    using ProcessingTools.Processors.Models.Abbreviations;
    using ProcessingTools.Xml.Contracts.Wrappers;

    public class AbbreviationsTagger : IAbbreviationsTagger
    {
        private const string SelectNodesToTagAbbreviationsXPathTemplate = ".//node()[contains(string(.),string('{0}'))]";

        private readonly IAbbreviationsHarvester abbreviationsHarvester;
        private readonly IXmlContextWrapper contextWrapper;
        private readonly ILogger logger;

        public AbbreviationsTagger(
            IAbbreviationsHarvester abbreviationsHarvester,
            IXmlContextWrapper contextWrapper,
            ILogger logger)
        {
            this.abbreviationsHarvester = abbreviationsHarvester ?? throw new ArgumentNullException(nameof(abbreviationsHarvester));
            this.contextWrapper = contextWrapper ?? throw new ArgumentNullException(nameof(contextWrapper));
            this.logger = logger;
        }

        public async Task<object> Tag(XmlNode context)
        {
            // Do not change this sequence
            await this.TagAbbreviationsInSubContextSelectedByXPathWithHarvestSubContext(
                context,
                "//graphic | //media | //disp-formula-group");

            await this.TagAbbreviationsInSubContextSelectedByXPathWithHarvestSubContext(
                context,
                "//chem-struct-wrap | //fig | //supplementary-material | //table-wrap");

            await this.TagAbbreviationsInSubContextSelectedByXPathWithHarvestSubContext(
                context,
                "//fig-group | //table-wrap-group");

            await this.TagAbbreviationsInSubContextSelectedByXPathWithHarvestSubContext(
                context,
                "//boxed-text");

            await this.TagAbbreviationsInSubContextSelectedByXPathWithHarvestContext(
                context,
                "//alt-title | //article-title | //attrib | //award-id | //comment | //conf-theme | //def-head | //funding-source | //license-p | //meta-value | //p | //preformat | //product | //subtitle | //supplement | //td | //term | //term-head | //th | //title | //trans-subtitle | //trans-title | //verse-line");

            return true;
        }

        private async Task TagAbbreviationsInSubContextSelectedByXPathWithHarvestSubContext(XmlNode context, string selectContextToTagXPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrWhiteSpace(selectContextToTagXPath))
            {
                throw new ArgumentNullException(nameof(selectContextToTagXPath));
            }

            var tasks = context.SelectNodes(selectContextToTagXPath)
                .Cast<XmlNode>()
                .Select(n => this.TagAbbreviationsWithHarvestWholeContext(n))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        private async Task<object> TagAbbreviationsWithHarvestWholeContext(XmlNode context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var abbreviationDefinitions = await this.GetAbbreviationCollection(context);
            var result = await this.TagAbbreviations(context, abbreviationDefinitions);
            return result;
        }

        private async Task TagAbbreviationsInSubContextSelectedByXPathWithHarvestContext(XmlNode context, string selectContextToTagXPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrWhiteSpace(selectContextToTagXPath))
            {
                throw new ArgumentNullException(nameof(selectContextToTagXPath));
            }

            var abbreviationDefinitions = await this.GetAbbreviationCollection(context);

            var tasks = context.SelectNodes(selectContextToTagXPath)
                 .Cast<XmlNode>()
                 .Select(n => this.TagAbbreviations(n, abbreviationDefinitions))
                 .ToArray();

            await Task.WhenAll(tasks);
        }

        private Task<object> TagAbbreviations(XmlNode context, IEnumerable<IAbbreviation> abbreviationDefinitions) => Task.Run<object>(() =>
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (abbreviationDefinitions == null || abbreviationDefinitions.Count() < 1)
            {
                return 0;
            }

            var document = this.contextWrapper.Create(context);

            var abbreviationSet = new HashSet<IAbbreviation>(abbreviationDefinitions);
            abbreviationSet.OrderByDescending(a => a.Content.Length)
                .ToList()
                .ForEach(abbreviation =>
                {
                    string xpath = string.Format(SelectNodesToTagAbbreviationsXPathTemplate, abbreviation.Content);
                    foreach (XmlNode node in document.SelectNodes(xpath))
                    {
                        bool canPerformReplace = node.CheckIfIsPossibleToPerformReplaceInXmlNode();
                        if (canPerformReplace)
                        {
                            try
                            {
                                node.ReplaceWholeXmlNodeByRegexPattern(abbreviation.SearchPattern, abbreviation.ReplacePattern);
                            }
                            catch (XmlException)
                            {
                                this.logger?.Log("Exception in abbreviation {0}", abbreviation.Content);
                            }
                        }
                    }
                });

            context.InnerXml = document.DocumentElement.InnerXml;

            return abbreviationSet.Count;
        });

        private async Task<IEnumerable<IAbbreviation>> GetAbbreviationCollection(XmlNode contextToHarvest)
        {
            if (contextToHarvest == null)
            {
                throw new ArgumentNullException(nameof(contextToHarvest));
            }

            var abbreviations = await this.abbreviationsHarvester.Harvest(contextToHarvest);
            if (abbreviations != null)
            {
                var result = abbreviations
                    .Where(a => !string.IsNullOrWhiteSpace(a.Value))
                    .Where(a => !string.IsNullOrWhiteSpace(a.Definition))
                    .Where(a => a.Value.Length > 1)
                    .Select(a => new Abbreviation(a.Value, a.ContentType, a.Definition))
                    .ToList();

                return result;
            }

            return null;
        }
    }
}
