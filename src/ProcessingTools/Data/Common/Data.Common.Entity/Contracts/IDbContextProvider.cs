﻿namespace ProcessingTools.Data.Common.Entity.Contracts
{
    using System.Data.Entity;
    using ProcessingTools.Contracts.Data;

    public interface IDbContextProvider<TContext> : IDatabaseProvider<TContext>
        where TContext : DbContext
    {
    }
}
