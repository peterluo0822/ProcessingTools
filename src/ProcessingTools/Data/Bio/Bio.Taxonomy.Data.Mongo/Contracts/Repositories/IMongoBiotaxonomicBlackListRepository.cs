﻿namespace ProcessingTools.Bio.Taxonomy.Data.Mongo.Contracts.Repositories
{
    using ProcessingTools.Contracts.Data.Repositories.Bio.Taxonomy;
    using ProcessingTools.Contracts.Models.Bio.Taxonomy;
    using ProcessingTools.Data.Common.Mongo.Contracts.Repositories;

    public interface IMongoBiotaxonomicBlackListRepository : IMongoCrudRepository<IBlackListEntity>, IBiotaxonomicBlackListRepository
    {
    }
}
