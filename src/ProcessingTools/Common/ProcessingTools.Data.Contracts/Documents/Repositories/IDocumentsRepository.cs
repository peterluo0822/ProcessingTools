﻿namespace ProcessingTools.Contracts.Data.Documents.Repositories
{
    using ProcessingTools.Contracts.Data.Repositories;
    using ProcessingTools.Models.Contracts.Documents;

    public interface IDocumentsRepository : ICrudRepository<IDocument>
    {
    }
}
