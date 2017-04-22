﻿namespace ProcessingTools.Contracts.Services.Data.Geo.Services
{
    using ProcessingTools.Contracts.Services.Data.Geo.Filters;
    using ProcessingTools.Contracts.Services.Data.Geo.Models;

    public interface IProvinceSynonymsDataService : IDataServiceAsync<IProvinceSynonym, IProvinceSynonymsFilter>
    {
    }
}