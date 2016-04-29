﻿namespace ProcessingTools.Bio.Taxonomy.Data.Contracts
{
    using ProcessingTools.Data.Common.Entity.Contracts;

    public interface IBioTaxonomyDataInitializer : IDbContextInitializer<TaxonomyDbContext>
    {
    }
}