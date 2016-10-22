﻿namespace ProcessingTools.Bio.Taxonomy.Processors.Contracts.Parsers
{
    using ProcessingTools.Bio.Taxonomy.Contracts;
    using ProcessingTools.Bio.Taxonomy.Services.Data.Contracts;
    using ProcessingTools.Contracts;

    public interface ITreatmentMetaParser<TService> : IDocumentParser
        where TService : ITaxaInformationResolverDataService<ITaxonClassification>
    {
    }
}