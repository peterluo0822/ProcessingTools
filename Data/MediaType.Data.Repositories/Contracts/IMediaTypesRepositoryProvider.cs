﻿namespace ProcessingTools.MediaType.Data.Repositories.Contracts
{
    using ProcessingTools.Data.Common.Contracts;
    using ProcessingTools.Data.Common.Repositories.Contracts;

    public interface IMediaTypesRepositoryProvider<T> : IGenericRepositoryProvider<IGenericRepository<T>, T>
        where T : class
    {
    }
}