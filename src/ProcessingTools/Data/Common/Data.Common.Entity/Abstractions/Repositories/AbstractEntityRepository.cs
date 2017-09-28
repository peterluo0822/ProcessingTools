﻿namespace ProcessingTools.Data.Common.Entity.Abstractions.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts;
    using ProcessingTools.Data.Common.Entity.Contracts;
    using ProcessingTools.Data.Common.Entity.Contracts.Repositories;
    using ProcessingTools.Data.Common.Expressions;
    using ProcessingTools.Data.Contracts.Repositories;

    public abstract class AbstractEntityRepository<TEntity, TContext, TDbModel> : ICrudRepository<TEntity>
        where TEntity : class
        where TContext : IDbContext
        where TDbModel : class, TEntity
    {
        private readonly IGenericRepository<TContext, TDbModel> repository;

        protected AbstractEntityRepository(IGenericRepository<TContext, TDbModel> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public virtual IQueryable<TEntity> Query => this.repository.DbSet.AsQueryable<TEntity>();

        protected abstract Func<TEntity, TDbModel> MapEntityToDbModel { get; }

        protected IGenericRepository<TContext, TDbModel> Repository => this.repository;

        public virtual Task<object> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var model = this.MapEntityToDbModel(entity);

            this.repository.Add(model);

            return Task.FromResult<object>(model);
        }

        public virtual Task<long> CountAsync()
        {
            return this.repository.DbSet.LongCountAsync();
        }

        public virtual Task<long> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return this.repository.DbSet.LongCountAsync(filter);
        }

        public virtual Task<object> DeleteAsync(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.repository.Delete(id);
            return Task.FromResult(id);
        }

        public virtual Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var query = this.repository.DbSet.Where(filter);

            return Task.FromResult(query.AsEnumerable());
        }

        public virtual Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return this.repository.DbSet.FirstOrDefaultAsync(filter);
        }

        public virtual Task<TEntity> GetByIdAsync(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var entity = this.repository.Get(id);
            return Task.FromResult<TEntity>(entity);
        }

        public virtual object SaveChanges() => this.repository.Context.SaveChanges();

        public virtual async Task<object> SaveChangesAsync() => await this.repository.Context.SaveChangesAsync().ConfigureAwait(false);

        public virtual Task<object> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var model = this.MapEntityToDbModel(entity);

            this.repository.Update(model);

            return Task.FromResult<object>(model);
        }

        public virtual async Task<object> UpdateAsync(object id, IUpdateExpression<TEntity> updateExpression)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (updateExpression == null)
            {
                throw new ArgumentNullException(nameof(updateExpression));
            }

            var model = this.repository.Get(id);
            if (model == null)
            {
                return null;
            }

            // TODO : Updater
            var updater = new Updater<TEntity>(updateExpression);
            await updater.Invoke(model).ConfigureAwait(false);

            this.repository.Update(model);

            return Task.FromResult<object>(model);
        }
    }
}
