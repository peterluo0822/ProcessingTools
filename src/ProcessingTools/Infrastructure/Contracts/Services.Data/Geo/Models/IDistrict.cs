﻿namespace ProcessingTools.Contracts.Services.Data.Geo.Models
{
    using System.Collections.Generic;
    using ProcessingTools.Contracts.Models;

    public interface IDistrict : ISynonymisable<IDistrictSynonym>, INameableIntegerIdentifiable, IServiceModel
    {
        int CountryId { get; }

        int? StateId { get; }

        int? ProvinceId { get; }

        int? RegionId { get; }

        ICollection<IMunicipality> Municipalities { get; }

        ICollection<ICounty> Counties { get; }

        ICollection<ICity> Cities { get; }
    }
}