﻿namespace ProcessingTools.Documents.Data.Common.Contracts.Repositories
{
    using Models;
    using ProcessingTools.Contracts.Data.Repositories;

    public interface IPublishersRepository : IAddressableRepository, ICrudRepository<IPublisherEntity>
    {
    }
}
