﻿namespace ProcessingTools.Data.Common.Mongo.Repositories.Contracts
{
    using ProcessingTools.Data.Common.Repositories.Contracts;

    public interface IMongoCrudRepository<T> : ICrudRepository<T>, IMongoRepository<T>
        where T : class
    {
    }
}