﻿namespace ProcessingTools.Web.ApiControllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using ProcessingTools.Contracts.Services.Data.Geo;
    using ProcessingTools.Web.Models.Countries;

    [Authorize]
    public class CountriesController : ApiController
    {
        private readonly ICountriesDataService service;

        public CountriesController(ICountriesDataService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: api/Countries
        public async Task<IEnumerable<CountryResponseModel>> Get()
        {
            var items = await this.service.SelectAsync(null);

            return items.Select(c => new CountryResponseModel
            {
                Id = c.Id,
                Name = c.Name,
                LanguageCode = c.LanguageCode
            })
            .ToArray();
        }
    }
}
