﻿namespace ProcessingTools.Data.Common.Entity.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using ProcessingTools.Data.Common.Entity.Contracts;
    using ProcessingTools.Data.Common.Entity.Models.Contracts;
    using ProcessingTools.Data.Common.Entity.Repositories.Contracts;

    public class EntityPreJoinedGenericRepository<TContext, TEntity> : EntityGenericRepository<TContext, TEntity>, IEntityGenericRepository<TEntity>, IDisposable
        where TContext : DbContext
        where TEntity : class, IEntityWithPreJoinedFields
    {
        private IEnumerable<string> prejoinFields;

        public EntityPreJoinedGenericRepository(IDbContextProvider<TContext> contextProvider)
            : base(contextProvider)
        {
            var entity = Activator.CreateInstance<TEntity>();
            this.prejoinFields = entity.PreJoinFieldNames?.ToArray();
        }

        public override IQueryable<TEntity> Query
        {
            get
            {
                var query = this.DbSet.AsQueryable();

                if (this.prejoinFields != null)
                {
                    foreach (var fieldName in this.prejoinFields)
                    {
                        query = query.Include(fieldName);
                    }
                }

                return query;
            }
        }

        public override async Task<TEntity> FindFirst(Expression<Func<TEntity, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return await this.Query.FirstOrDefaultAsync(filter);
        }
    }
}
