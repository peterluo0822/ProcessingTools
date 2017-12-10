﻿namespace ProcessingTools.Tagger.Commands.Generics
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;
    using ProcessingTools.Common.Extensions;
    using ProcessingTools.Contracts;
    using ProcessingTools.Contracts.Commands;
    using ProcessingTools.Contracts.Commands.Tagger;
    using ProcessingTools.Contracts.Models.Bio.Taxonomy;
    using ProcessingTools.Contracts.Processors.Processors.Bio.Taxonomy.Parsers;
    using ProcessingTools.Contracts.Services.Data.Bio.Taxonomy;

    public class GenericParseHigherTaxaCommand<TService> : ITaggerCommand
        where TService : ITaxaRankResolver
    {
        private readonly IHigherTaxaParserWithDataService<TService, ITaxonRank> parser;
        private readonly IReporter reporter;

        public GenericParseHigherTaxaCommand(
            IHigherTaxaParserWithDataService<TService, ITaxonRank> parser,
            IReporter reporter)
        {
            this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
            this.reporter = reporter ?? throw new ArgumentNullException(nameof(reporter));
        }

        public async Task<object> RunAsync(IDocument document, ICommandSettings settings)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var result = await this.parser.ParseAsync(document.XmlDocument.DocumentElement).ConfigureAwait(false);

            await this.PrintNonParsedTaxa(document.XmlDocument).ConfigureAwait(false);

            return result;
        }

        private async Task PrintNonParsedTaxa(XmlNode node)
        {
            var uniqueHigherTaxaList = node.ExtractUniqueNonParsedHigherTaxa()
                .Distinct()
                .OrderBy(s => s)
                .ToArray();

            if (uniqueHigherTaxaList.Length > 0)
            {
                this.reporter.AppendContent("Non-parsed taxa:");
                foreach (var taxonName in uniqueHigherTaxaList)
                {
                    this.reporter.AppendContent(string.Format("\t{0}", taxonName));
                }

                await this.reporter.MakeReportAsync().ConfigureAwait(false);
            }
        }
    }
}
