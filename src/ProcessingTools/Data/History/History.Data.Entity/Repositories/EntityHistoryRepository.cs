﻿namespace ProcessingTools.History.Data.Entity.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts.Data.History.Models;
    using ProcessingTools.Contracts.Expressions;
    using ProcessingTools.Data.Common.Entity.Repositories;
    using ProcessingTools.History.Data.Entity.Contracts;
    using ProcessingTools.History.Data.Entity.Contracts.Repositories;
    using ProcessingTools.History.Data.Entity.Models;

    public class EntityHistoryRepository : GenericRepository<IHistoryDbContext, HistoryItem>, IEntityHistoryRepository
    {
        public EntityHistoryRepository(IHistoryDbContext context)
            : base(context)
        {
        }

        public IQueryable<IHistoryItem> Query => this.DbSet.AsQueryable<IHistoryItem>();

        public Task<object> Add(IHistoryItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var model = new HistoryItem
            {
                Data = entity.Data,
                DateModified = entity.DateModified,
                ObjectId = entity.ObjectId,
                ObjectType = entity.ObjectType,
                UserId = entity.UserId
            };

            base.Add(model);

            return Task.FromResult<object>(model.Id);
        }

        public Task<long> Count()
        {
            var result = this.DbSet.LongCount();

            return Task.FromResult(result);
        }

        public Task<long> Count(Expression<Func<IHistoryItem, bool>> filter)
        {
            var result = this.DbSet.AsQueryable<IHistoryItem>().LongCount(filter);

            return Task.FromResult(result);
        }

        public new Task<object> Delete(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            base.Delete(id: id);

            return Task.FromResult(id);
        }

        public Task<IEnumerable<IHistoryItem>> Find(Expression<Func<IHistoryItem, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var query = this.DbSet.AsQueryable<IHistoryItem>().Where(filter);
            var result = query.AsEnumerable();

            return Task.FromResult(result);
        }

        public Task<IHistoryItem> FindFirst(Expression<Func<IHistoryItem, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var query = this.DbSet.AsQueryable<IHistoryItem>().Where(filter);
            var result = query.FirstOrDefault();

            return Task.FromResult(result);
        }

        public Task<IHistoryItem> GetById(object id)
        {
            var entity = this.DbSet.Find(id);
            return Task.FromResult<IHistoryItem>(entity);
        }

        public Task<object> Update(IHistoryItem entity)
        {
            throw new InvalidOperationException();
        }

        public Task<object> Update(object id, IUpdateExpression<IHistoryItem> update)
        {
            throw new InvalidOperationException();
        }
    }
}
