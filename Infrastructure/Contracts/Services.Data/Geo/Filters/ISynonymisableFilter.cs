﻿namespace ProcessingTools.Contracts.Services.Data.Geo.Filters
{
    public interface ISynonymisableFilter : IGeoFilter
    {
        string Synonym { get; }
    }
}
