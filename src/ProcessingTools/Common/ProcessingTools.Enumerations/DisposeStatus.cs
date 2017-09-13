﻿// <copyright file="DisposeStatus.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Enumerations
{
    /// <summary>
    /// Dispose status
    /// </summary>
    public enum DisposeStatus
    {
        /// <summary>
        /// Object can not be disposed
        /// </summary>
        NotDisposable = 0,

        /// <summary>
        /// Object is disposed
        /// </summary>
        Disposed = 1,

        /// <summary>
        /// Object is not disposed
        /// </summary>
        NotDisposed = 2
    }
}