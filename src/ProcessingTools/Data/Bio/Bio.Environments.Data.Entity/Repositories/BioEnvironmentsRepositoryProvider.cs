﻿namespace ProcessingTools.Bio.Environments.Data.Entity.Repositories
{
    using System;
    using ProcessingTools.Bio.Environments.Data.Entity.Contracts;
    using ProcessingTools.Bio.Environments.Data.Entity.Contracts.Repositories;
    using ProcessingTools.Contracts.Data.Repositories;

    public class BioEnvironmentsRepositoryProvider<T> : IBioEnvironmentsRepositoryProvider<T>
        where T : class
    {
        private IBioEnvironmentsDbContextProvider contextProvider;

        public BioEnvironmentsRepositoryProvider(IBioEnvironmentsDbContextProvider contextProvider)
        {
            if (contextProvider == null)
            {
                throw new ArgumentNullException(nameof(contextProvider));
            }

            this.contextProvider = contextProvider;
        }

        public ICrudRepository<T> Create()
        {
            return new BioEnvironmentsRepository<T>(this.contextProvider);
        }
    }
}