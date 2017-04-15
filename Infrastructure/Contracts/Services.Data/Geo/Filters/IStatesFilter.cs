﻿namespace ProcessingTools.Contracts.Services.Data.Geo.Filters
{
    public interface IStatesFilter : ISynonymisableFilter
    {
        string Country { get; }

        string Province { get; }

        string Region { get; }

        string District { get; }

        string Municipality { get; }

        string County { get; }

        string City { get; }
    }
}
