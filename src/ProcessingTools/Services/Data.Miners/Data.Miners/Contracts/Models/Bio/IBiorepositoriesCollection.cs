﻿namespace ProcessingTools.Data.Miners.Contracts.Models.Bio
{
    public interface IBiorepositoriesCollection
    {
        string CollectionCode { get; }

        string CollectionName { get; }

        string Url { get; }
    }
}
