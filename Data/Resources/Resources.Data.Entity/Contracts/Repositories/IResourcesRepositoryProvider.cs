﻿namespace ProcessingTools.DataResources.Data.Entity.Contracts.Repositories
{
    using ProcessingTools.Contracts.Data.Repositories;

    public interface IResourcesRepositoryProvider<T> : ISearchableCountableCrudRepositoryProvider<T>
        where T : class
    {
    }
}
