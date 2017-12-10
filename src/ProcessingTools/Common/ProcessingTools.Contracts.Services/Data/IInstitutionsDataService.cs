﻿// <copyright file="IInstitutionsDataService.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Services.Data.Contracts
{
    using ProcessingTools.Contracts.Models;
    using ProcessingTools.Contracts.Models.Resources;
    using ProcessingTools.Contracts.Services.Data;

    public interface IInstitutionsDataService : IMultiDataServiceAsync<IInstitution, IFilter>
    {
    }
}