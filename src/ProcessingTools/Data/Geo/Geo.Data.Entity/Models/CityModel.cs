﻿namespace ProcessingTools.Geo.Data.Entity.Models
{
    using System.Collections.Generic;
    using ProcessingTools.Contracts.Models.Geo;

    internal class CityModel : ICity
    {
        public int CountryId { get; set; }

        public ICountry Country { get; set; }

        public int? CountyId { get; set; }

        public int? DistrictId { get; set; }

        public int Id { get; set; }

        public int? MunicipalityId { get; set; }

        public string Name { get; set; }

        public string AbbreviatedName { get; set; }

        public ICollection<IPostCode> PostCodes { get; set; } = new HashSet<IPostCode>();

        public int? ProvinceId { get; set; }

        public int? RegionId { get; set; }

        public int? StateId { get; set; }

        public ICollection<ICitySynonym> Synonyms { get; set; } = new HashSet<ICitySynonym>();
    }
}
