﻿namespace ProcessingTools.Geo.Data.Miners
{
    using Contracts;
    using ProcessingTools.Data.Miners.Common;
    using ProcessingTools.Geo.Services.Data.Contracts;
    using ProcessingTools.Geo.Services.Data.Models;

    public class GeoEpithetsDataMiner : SimpleServiceStringDataMiner<IGeoEpithetsDataService, GeoEpithetServiceModel>, IGeoEpithetsDataMiner
    {
        public GeoEpithetsDataMiner(IGeoEpithetsDataService service)
            : base(service)
        {
        }
    }
}