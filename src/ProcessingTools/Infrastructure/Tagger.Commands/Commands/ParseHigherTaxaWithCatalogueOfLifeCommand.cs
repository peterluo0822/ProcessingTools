﻿namespace ProcessingTools.Tagger.Commands.Commands
{
    using System.ComponentModel;
    using ProcessingTools.Contracts;
    using ProcessingTools.Contracts.Models.Bio.Taxonomy;
    using ProcessingTools.Processors.Contracts.Processors.Bio.Taxonomy.Parsers;
    using ProcessingTools.Services.Data.Contracts.Bio.Taxonomy;
    using ProcessingTools.Tagger.Commands.Contracts.Commands;
    using ProcessingTools.Tagger.Commands.Generics;

    [Description("Parse higher taxa using CoL.")]
    public class ParseHigherTaxaWithCatalogueOfLifeCommand : GenericParseHigherTaxaCommand<ICatalogueOfLifeTaxaRankResolver>, IParseHigherTaxaWithCatalogueOfLifeCommand
    {
        public ParseHigherTaxaWithCatalogueOfLifeCommand(
            IHigherTaxaParserWithDataService<ICatalogueOfLifeTaxaRankResolver, ITaxonRank> parser,
            IReporter reporter)
            : base(parser, reporter)
        {
        }
    }
}
