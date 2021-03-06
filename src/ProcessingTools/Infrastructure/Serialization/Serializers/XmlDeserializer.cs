﻿namespace ProcessingTools.Serialization.Serializers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using Contracts;

    public class XmlDeserializer : IXmlDeserializer
    {
        public Task<T> Deserialize<T>(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var serializer = new XmlSerializer(typeof(T));

            return Task.Run(() =>
            {
                stream.Position = 0;

                var result = serializer.Deserialize(stream);
                return (T)result;
            });
        }
    }
}
