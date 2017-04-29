﻿namespace ProcessingTools.Geo.Services.Data.Services
{
    using System;
    using System.Linq.Expressions;
    using ProcessingTools.Geo.Data.Entity.Contracts.Repositories;
    using ProcessingTools.Geo.Data.Entity.Models;
    using ProcessingTools.Geo.Services.Data.Abstractions.Services;
    using ProcessingTools.Geo.Services.Data.Contracts.Models;
    using ProcessingTools.Geo.Services.Data.Contracts.Services;
    using ProcessingTools.Geo.Services.Data.Models;

    public class CountriesSelectableDataService : AbstractSelectableDataService<ICountryListableModel, Country>, ICountriesSelectableDataService
    {
        public CountriesSelectableDataService(IGeoDataRepository<Country> repository)
            : base(repository)
        {
        }

        protected override Expression<Func<Country, ICountryListableModel>> MapDataModelToServiceModel => c => new CountryListableModel
        {
            Id = c.Id,
            Name = c.Name
        };
    }
}