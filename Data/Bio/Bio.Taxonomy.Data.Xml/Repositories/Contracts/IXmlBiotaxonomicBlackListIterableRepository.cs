﻿namespace ProcessingTools.Bio.Taxonomy.Data.Xml.Repositories.Contracts
{
    using ProcessingTools.Bio.Taxonomy.Data.Common.Models.Contracts;
    using ProcessingTools.Bio.Taxonomy.Data.Common.Repositories.Contracts;
    using ProcessingTools.Data.Common.File.Contracts.Repositories;

    public interface IXmlBiotaxonomicBlackListIterableRepository : IFileIterableRepository<IBlackListEntity>, IBiotaxonomicBlackListIterableRepository
    {
    }
}
