﻿namespace ProcessingTools.Documents.Data.Repositories.Contracts
{
    using ProcessingTools.Data.Common.Repositories.Contracts;

    public interface IDocumentsRepositoryProvider<T> : IGenericRepositoryProvider<T>
        where T : class
    {
    }
}
