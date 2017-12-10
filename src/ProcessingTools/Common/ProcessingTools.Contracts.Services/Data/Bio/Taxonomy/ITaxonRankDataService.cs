﻿// <copyright file="ITaxonRankDataService.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Contracts.Services.Data.Bio.Taxonomy
{
    using ProcessingTools.Contracts.Models.Bio.Taxonomy;
    using ProcessingTools.Contracts.Services.Data;

    public interface ITaxonRankDataService : IAddableDataService<ITaxonRank>, IDeletableDataService<ITaxonRank>
    {
    }
}