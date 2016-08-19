﻿namespace ProcessingTools.Bio.Taxonomy.Data.Xml.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Contracts;
    using Models;

    using ProcessingTools.Bio.Taxonomy.Data.Common.Models.Contracts;
    using ProcessingTools.Bio.Taxonomy.Data.Xml.Contracts;
    using ProcessingTools.Common.Exceptions;
    using ProcessingTools.Data.Common.Expressions;
    using ProcessingTools.Data.Common.Expressions.Contracts;

    public class XmlBiotaxonomicBlackListRepository : IXmlBiotaxonomicBlackListRepository
    {
        private readonly IXmlBiotaxonomicBlackListContext context;

        public XmlBiotaxonomicBlackListRepository(IXmlBiotaxonomicBlackListContextProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            this.context = provider.Create();
        }

        public virtual async Task<object> Add(IBlackListEntity entity)
        {
            if (entity == null || string.IsNullOrWhiteSpace(entity.Content))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = await this.context.Add(entity.Content);

            return result;
        }

        public virtual async Task<object> Delete(object id)
        {
            if (id == null || string.IsNullOrWhiteSpace(id.ToString()))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var result = await this.context.Delete(id.ToString());

            return result;
        }

        public virtual Task<object> Delete(IBlackListEntity entity)
        {
            if (entity == null || string.IsNullOrWhiteSpace(entity.Content))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return this.Delete(entity.Content);
        }

        public virtual async Task<IBlackListEntity> Get(object id)
        {
            if (id == null || string.IsNullOrWhiteSpace(id.ToString()))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var query = await this.context.All();

            var result = query.Where(s => s == id.ToString())
                .Select(s => new BlackListEntity
                {
                    Content = s
                })
                .FirstOrDefault();

            return result;
        }

        public virtual Task<long> SaveChanges() => this.context.WriteItemsToFile();

        public virtual Task<object> Update(IBlackListEntity entity) => this.Add(entity);

        public virtual async Task<object> Update(object id, IUpdateExpression<IBlackListEntity> update)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (update == null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            var entity = await this.Get(id);
            if (entity == null)
            {
                throw new EntityNotFoundException();
            }

            await this.Delete(entity);

            // TODO : Updater
            var updater = new Updater<IBlackListEntity>(update);
            await updater.Invoke(entity);

            return await this.Add(entity);
        }
    }
}
