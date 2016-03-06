﻿namespace ProcessingTools.Geo.Data.Repositories.Contracts
{
    using ProcessingTools.Data.Common.Entity.Repositories.Contracts;

    public interface IGeoDataRepository<T> : IEfRepository<T>
        where T : class
    {
    }
}