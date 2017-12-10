﻿// <copyright file="IMediaType.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Contracts.Processors.Models.Floats
{
    /// <summary>
    /// Media-type.
    /// </summary>
    public interface IMediaType
    {
        /// <summary>
        /// Gets file extension.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Gets mime-subtype.
        /// </summary>
        string MimeSubtype { get; }

        /// <summary>
        /// Gets mime-type;
        /// </summary>
        string MimeType { get; }
    }
}