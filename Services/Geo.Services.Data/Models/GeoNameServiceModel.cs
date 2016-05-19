﻿namespace ProcessingTools.Geo.Services.Data.Models
{
    using ProcessingTools.Services.Common.Models.Contracts;

    public class GeoNameServiceModel : INamedDataServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
