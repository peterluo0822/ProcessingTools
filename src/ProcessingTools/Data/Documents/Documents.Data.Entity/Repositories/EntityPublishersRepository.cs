﻿namespace ProcessingTools.Documents.Data.Entity.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts.Data.Documents.Models;
    using ProcessingTools.Documents.Data.Entity.Contracts;
    using ProcessingTools.Documents.Data.Entity.Contracts.Repositories;
    using ProcessingTools.Documents.Data.Entity.Models;

    public class EntityPublishersRepository : EntityAddressableRepository<Publisher, IPublisherEntity>, IEntityPublishersRepository
    {
        public EntityPublishersRepository(IDocumentsDbContextProvider contextProvider)
            : base(contextProvider)
        {
        }

        protected override Func<IPublisherEntity, Publisher> MapEntityToDbModel => e => new Publisher(e);

        public override async Task<object> Add(IPublisherEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var dbmodel = new Publisher(entity);
            foreach (var entityAddress in entity.Addresses)
            {
                var dbaddress = await this.AddOrGetAddress(entityAddress);
                dbmodel.Addresses.Add(dbaddress);
            }

            return await this.Add(dbmodel, this.DbSet);
        }

        public override Task<long> Count() => this.DbSet.LongCountAsync();

        public override Task<long> Count(Expression<Func<IPublisherEntity, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var query = this.DbSet.AsQueryable<IPublisherEntity>();
            return query.LongCountAsync(filter);
        }

        public override async Task<IPublisherEntity> GetById(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var query = this.DbSet
                .Include(p => p.Addresses)
                .Where(p => p.Id.ToString() == id.ToString());

            var entity = await query.FirstOrDefaultAsync();

            return entity;
        }
    }
}
