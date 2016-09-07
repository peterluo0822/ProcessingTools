﻿namespace ProcessingTools.Bio.Taxonomy.Data.Xml.Repositories
{
    using System;

    using Contracts;

    using ProcessingTools.Bio.Taxonomy.Data.Common.Models.Contracts;
    using ProcessingTools.Bio.Taxonomy.Data.Xml.Contracts;
    using ProcessingTools.Configurator;
    using ProcessingTools.Data.Common.Repositories.Contracts;

    public class XmlBiotaxonomicBlackListRepositoryProvider : IXmlBiotaxonomicBlackListRepositoryProvider
    {
        private readonly IXmlBiotaxonomicBlackListContextProvider contextProvider;

        public XmlBiotaxonomicBlackListRepositoryProvider(IXmlBiotaxonomicBlackListContextProvider contextProvider)
        {
            if (contextProvider == null)
            {
                throw new ArgumentNullException(nameof(contextProvider));
            }

            this.contextProvider = contextProvider;
        }

        public ICrudRepository<IBlackListEntity> Create()
        {
            return new XmlBiotaxonomicBlackListRepository(this.contextProvider, ConfigBuilder.Create());
        }
    }
}