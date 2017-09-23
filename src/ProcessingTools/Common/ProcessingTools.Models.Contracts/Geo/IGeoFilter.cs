﻿namespace ProcessingTools.Contracts.Filters.Geo
{
    using ProcessingTools.Models.Contracts;

    public interface IGeoFilter : INameable, IGenericIdentifiable<int?>, IFilter
    {
    }
}
