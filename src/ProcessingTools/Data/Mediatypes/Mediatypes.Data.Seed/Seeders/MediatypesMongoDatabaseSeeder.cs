﻿namespace ProcessingTools.Mediatypes.Data.Seed.Seeders
{
    using System;
    using System.Collections.Concurrent;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using Newtonsoft.Json;
    using ProcessingTools.Constants.Configuration;
    using ProcessingTools.Data.Common.Mongo.Contracts;
    using ProcessingTools.Data.Common.Mongo.Factories;
    using ProcessingTools.Mediatypes.Data.Mongo.Models;
    using ProcessingTools.Mediatypes.Data.Seed.Contracts;
    using ProcessingTools.Mediatypes.Data.Seed.Models;

    public class MediatypesMongoDatabaseSeeder : IMediatypesMongoDatabaseSeeder
    {
        private readonly IMongoDatabase db;

        private ConcurrentQueue<Exception> exceptions;

        public MediatypesMongoDatabaseSeeder(IMongoDatabaseProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            this.db = provider.Create();
            this.exceptions = new ConcurrentQueue<Exception>();
        }

        public async Task<object> Seed()
        {
            this.exceptions = new ConcurrentQueue<Exception>();

            ExtensionJson[] mediatypesJson = this.ParseDataJsonFile();

            if (mediatypesJson.Length < 1)
            {
                throw new ApplicationException("Mediatypes data json file is empty or invalid.");
            }

            await this.ImportMediatypesToDatabase(mediatypesJson);

            if (this.exceptions.Count > 0)
            {
                throw new AggregateException(this.exceptions);
            }

            return true;
        }

        private ExtensionJson[] ParseDataJsonFile()
        {
            string jsonFilePath = ConfigurationManager.AppSettings[AppSettingsKeys.MediaTypeDataJsonFileName];

            string jsonString = File.ReadAllText(jsonFilePath);

            var mediatypesJson = JsonConvert.DeserializeObject<ExtensionJson[]>(jsonString);
            return mediatypesJson?.Select(m => new ExtensionJson
            {
                Extension = m.Extension.ToLower(),
                Mimetype = m.Mimetype.ToLower(),
                Mimesubtype = m.Mimesubtype.ToLower(),
                Description = m.Description
            })
            .ToArray();
        }

        private async Task ImportMediatypesToDatabase(ExtensionJson[] mediatypes)
        {
            string collectionName = CollectionNameFactory.Create<Mediatype>();
            var collection = this.db.GetCollection<Mediatype>(collectionName);

            foreach (var mediatype in mediatypes)
            {
                try
                {
                    await collection.InsertOneAsync(new Mediatype
                    {
                        FileExtension = mediatype.Extension,
                        Description = mediatype.Description,
                        Mimetype = mediatype.Mimetype,
                        Mimesubtype = mediatype.Mimesubtype
                    });
                }
                catch (Exception e)
                {
                    this.exceptions.Enqueue(e);
                }
            }
        }
    }
}
