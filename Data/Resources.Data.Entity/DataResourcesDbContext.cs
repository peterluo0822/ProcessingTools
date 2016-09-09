﻿namespace ProcessingTools.Resources.Data.Entity
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using Models;

    public class DataResourcesDbContext : DbContext
    {
        public DataResourcesDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public IDbSet<ContentType> ContentTypes { get; set; }

        public IDbSet<SourceId> Sources { get; set; }

        public IDbSet<Product> Products { get; set; }

        public IDbSet<Institution> Institutions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
