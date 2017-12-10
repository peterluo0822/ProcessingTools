﻿// <copyright file="IDistrictsRepository.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Contracts.Data.Repositories.Geo
{
    using ProcessingTools.Contracts.Models.Geo;

    /// <summary>
    /// Districts repository.
    /// </summary>
    public interface IDistrictsRepository : IRepositoryAsync<IDistrict, IDistrictsFilter>, IGeoSynonymisableRepository<IDistrict, IDistrictSynonym, IDistrictSynonymsFilter>
    {
    }
}