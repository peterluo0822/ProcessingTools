﻿namespace ProcessingTools.Tagger.Commands.Commands
{
    using System.ComponentModel;
    using ProcessingTools.Contracts.Commands.Tagger;
    using ProcessingTools.Contracts.Processors.Processors.Bio.Taxonomy.Parsers;
    using ProcessingTools.Contracts.Services.Data.Bio.Taxonomy;
    using ProcessingTools.Tagger.Commands.Generics;

    [Description("Parse treatment meta with GBIF.")]
    public class ParseTreatmentMetaWithGbifCommand : GenericDocumentParserCommand<ITreatmentMetaParserWithDataService<IGbifTaxaClassificationResolver>>, IParseTreatmentMetaWithGbifCommand
    {
        public ParseTreatmentMetaWithGbifCommand(ITreatmentMetaParserWithDataService<IGbifTaxaClassificationResolver> parser)
            : base(parser)
        {
        }
    }
}
