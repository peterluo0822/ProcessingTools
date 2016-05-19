﻿namespace ProcessingTools.Bio.Taxonomy.Data
{
    using System;
    using Contracts;
    using ProcessingTools.Bio.Taxonomy.Data.Common.Constants;

    public class BioTaxonomyDbContextFactory : IBioTaxonomyDbContextFactory
    {
        private string connectionString;

        public BioTaxonomyDbContextFactory()
        {
            this.ConnectionString = ConnectionConstants.BioTaxonomyDbContextConnectionKey;
        }

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(this.ConnectionString));
                }

                this.connectionString = value;
            }
        }

        public BioTaxonomyDbContext Create()
        {
            return new BioTaxonomyDbContext(this.ConnectionString);
        }
    }
}