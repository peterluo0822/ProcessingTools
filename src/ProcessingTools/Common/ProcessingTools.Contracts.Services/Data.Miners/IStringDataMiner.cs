﻿// <copyright file="IStringDataMiner.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Contracts.Data.Miners
{
    /// <summary>
    /// String data miner.
    /// Data miner with string context and string output model.
    /// </summary>
    public interface IStringDataMiner : IDataMiner<string, string>
    {
    }
}