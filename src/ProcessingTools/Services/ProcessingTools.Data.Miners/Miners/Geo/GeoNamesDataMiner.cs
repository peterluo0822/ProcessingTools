﻿namespace ProcessingTools.Data.Miners.Miners.Geo
{
    using ProcessingTools.Contracts.Models;
    using ProcessingTools.Contracts.Models.Geo;
    using ProcessingTools.Contracts.Services.Data.Geo;
    using ProcessingTools.Data.Miners.Contracts.Miners.Geo;
    using ProcessingTools.Data.Miners.Generics;

    public class GeoNamesDataMiner : SimpleServiceStringDataMiner<IGeoNamesDataService, IGeoName, ITextFilter>, IGeoNamesDataMiner
    {
        public GeoNamesDataMiner(IGeoNamesDataService service)
            : base(service)
        {
        }
    }
}