﻿namespace ProcessingTools.Contracts.Data.Documents.Repositories
{
    using ProcessingTools.Contracts.Data.Documents.Models;
    using ProcessingTools.Contracts.Data.Repositories;

    public interface IInstitutionsRepository : IAddressableRepository, ICrudRepository<IInstitution>
    {
    }
}
