﻿namespace ProcessingTools.Data.Common.Repositories.Contracts
{
    using System.Threading.Tasks;
    using Expressions.Contracts;

    public interface ICrudRepository<TEntity> : IRepository<TEntity>
    {
        Task<object> Add(TEntity entity);

        Task<object> Delete(TEntity entity);

        Task<object> Delete(object id);

        Task<TEntity> Get(object id);

        Task<long> SaveChanges();

        Task<object> Update(TEntity entity);

        Task<object> Update(object id, IUpdateExpression<TEntity> update);
    }
}
