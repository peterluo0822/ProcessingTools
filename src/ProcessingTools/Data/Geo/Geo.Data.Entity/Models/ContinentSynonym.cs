﻿namespace ProcessingTools.Geo.Data.Entity.Models
{
    using ProcessingTools.Contracts.Models;

    public class ContinentSynonym : Synonym, IDataModel
    {
        public virtual int ContinentId { get; set; }

        public virtual Continent Continent { get; set; }
    }
}
