﻿// <copyright file="IRepositoryProvider.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Data.Contracts.Repositories
{
    public interface IRepositoryProvider<out TRepository>
        where TRepository : IRepository
    {
        TRepository Create();
    }
}