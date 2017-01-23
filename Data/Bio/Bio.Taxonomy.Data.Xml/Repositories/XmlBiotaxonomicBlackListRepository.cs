﻿namespace ProcessingTools.Bio.Taxonomy.Data.Xml.Repositories
{
    using System;
    using System.Threading.Tasks;
    using Contracts.Repositories;
    using ProcessingTools.Bio.Taxonomy.Data.Common.Contracts.Models;
    using ProcessingTools.Bio.Taxonomy.Data.Xml.Contracts;
    using ProcessingTools.Contracts;
    using ProcessingTools.Data.Common.File.Repositories;

    public class XmlBiotaxonomicBlackListRepository : FileGenericRepository<IXmlBiotaxonomicBlackListContext, IBlackListEntity>, IXmlBiotaxonomicBlackListRepository
    {
        private readonly string dataFileName;

        public XmlBiotaxonomicBlackListRepository(string dataFileName, IFactory<IXmlBiotaxonomicBlackListContext> contextFactory)
            : base(contextFactory)
        {
            if (string.IsNullOrWhiteSpace(dataFileName))
            {
                throw new ArgumentNullException(nameof(dataFileName));
            }

            this.dataFileName = dataFileName;

            this.Context.LoadFromFile(this.dataFileName).Wait();
        }

        public override Task<long> SaveChanges() => this.Context.WriteToFile(this.dataFileName);

        public override Task<object> Update(IBlackListEntity entity) => this.Add(entity);
    }
}
