﻿// <copyright file="IGeoSynonym.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Contracts.Models.Geo
{
    using ProcessingTools.Contracts.Models;

    /// <summary>
    /// Base synonym model for geo-objects.
    /// </summary>
    public interface IGeoSynonym : INameableIntegerIdentifiable
    {
        /// <summary>
        /// Gets parent ID.
        /// </summary>
        int ParentId { get; }

        /// <summary>
        /// Gets language code.
        /// </summary>
        int? LanguageCode { get; }
    }
}