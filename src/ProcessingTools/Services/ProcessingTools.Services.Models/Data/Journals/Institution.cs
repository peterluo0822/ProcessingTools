﻿// <copyright file="Institution.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Services.Models.Data.Journals
{
    using ProcessingTools.Services.Models.Contracts.Data.Journals;

    /// <summary>
    /// Institution.
    /// </summary>
    public class Institution : IInstitution
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string AbbreviatedName { get; set; }
    }
}
