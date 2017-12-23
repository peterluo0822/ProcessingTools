﻿// <copyright file="ITransformCache{T}.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Contracts.Xml
{
    /// <summary>
    /// Transform cache.
    /// </summary>
    /// <typeparam name="T">Type of the cached transform object.</typeparam>
    public interface ITransformCache<out T>
    {
        /// <summary>
        /// Gets instance by file name.
        /// </summary>
        /// <param name="fileName">Name of the cached file.</param>
        /// <returns>Cached instance corresponding to the specified file name.</returns>
        T this[string fileName] { get; }

        /// <summary>
        /// Removes single cached instance by its file name.
        /// </summary>
        /// <param name="fileName">Name of the cached file.</param>
        /// <returns>True if succeeded.</returns>
        bool Remove(string fileName);

        /// <summary>
        /// Removed all cached items.
        /// </summary>
        /// <returns>True if succeeded.</returns>
        bool RemoveAll();
    }
}