﻿namespace ProcessingTools.Serialization.Contracts
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IXmlDeserializer
    {
        Task<T> Deserialize<T>(Stream stream);
    }
}
