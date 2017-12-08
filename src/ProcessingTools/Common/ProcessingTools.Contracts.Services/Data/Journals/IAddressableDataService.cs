﻿// <copyright file="IAddressableDataService.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Journals.Services.Data.Contracts.Services
{
    using System.Threading.Tasks;
    using ProcessingTools.Contracts.Services.Models.Data.Journals;

    public interface IAddressableDataService<T>
        where T : IAddressable
    {
        Task<object> AddAddressAsync(object userId, object modelId, IAddress address);

        Task<object> UpdateAddressAsync(object userId, object modelId, IAddress address);

        Task<object> RemoveAddressAsync(object userId, object modelId, object addressId);
    }
}
