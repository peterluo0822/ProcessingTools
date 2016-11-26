﻿namespace ProcessingTools.Bio.Taxonomy.Data.Xml.Contracts
{
    using ProcessingTools.Bio.Taxonomy.Data.Common.Models.Contracts;
    using ProcessingTools.Contracts.Data;
    using ProcessingTools.Data.Common.File.Contracts;

    public interface ITaxaContextProvider : IDatabaseProvider<ITaxaContext>, IFileDbContextProvider<ITaxaContext, ITaxonRankEntity>
    {
    }
}