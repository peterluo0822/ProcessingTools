﻿// <copyright file="ITaxonRankEntity.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Contracts.Models.Bio.Taxonomy
{
    using System.Collections.Generic;
    using ProcessingTools.Contracts.Models;
    using ProcessingTools.Enumerations;

    /// <summary>
    /// Taxon rank entity.
    /// </summary>
    public interface ITaxonRankEntity : INameable
    {
        /// <summary>
        /// Gets a value indicating whether entity is included in the white list.
        /// </summary>
        bool IsWhiteListed { get; }

        /// <summary>
        /// Gets ranks.
        /// </summary>
        ICollection<TaxonRankType> Ranks { get; }
    }
}