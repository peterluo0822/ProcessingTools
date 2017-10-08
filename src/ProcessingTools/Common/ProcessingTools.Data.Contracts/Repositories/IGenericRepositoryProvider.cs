﻿// <copyright file="IGenericRepositoryProvider.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Data.Contracts.Repositories
{
    using System;
    using System.Threading.Tasks;

    public interface IGenericRepositoryProvider<TRepository>
        where TRepository : IRepository
    {
        Task Execute(Action<TRepository> action);

        Task Execute(Func<TRepository, Task> function);

        Task<T> Execute<T>(Func<TRepository, Task<T>> function);
    }
}