﻿// <copyright file="IAddressable.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Contracts.Data.Documents.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Model with addresses.
    /// </summary>
    public interface IAddressable
    {
        /// <summary>
        /// Gets addresses.
        /// </summary>
        IEnumerable<IAddress> Addresses { get; }
    }
}
