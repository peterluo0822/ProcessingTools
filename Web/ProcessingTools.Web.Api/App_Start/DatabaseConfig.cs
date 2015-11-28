﻿namespace ProcessingTools.Web.Api
{
    using System.Data.Entity;

    public static class DatabaseConfig
    {
        public static void Initialize()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ProcessingTools.Api.Data.ApplicationDbContext, ProcessingTools.Api.Data.Migrations.Configuration>());
            ////ProcessingTools.Api.Data.ApplicationDbContext.Create().Database.Initialize(true);

            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ProcessingTools.MediaType.Data.MediaTypesDbContext, ProcessingTools.MediaType.Data.Migrations.Configuration>());
            ////ProcessingTools.MediaType.Data.MediaTypesDbContext.Create().Database.Initialize(true);
        }
    }
}