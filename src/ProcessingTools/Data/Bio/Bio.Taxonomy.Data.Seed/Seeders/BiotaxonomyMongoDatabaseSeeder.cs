﻿namespace ProcessingTools.Bio.Taxonomy.Data.Seed.Seeders
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using ProcessingTools.Bio.Taxonomy.Data.Mongo.Contracts.Repositories;
    using ProcessingTools.Bio.Taxonomy.Data.Mongo.Models;
    using ProcessingTools.Bio.Taxonomy.Data.Seed.Contracts;
    using ProcessingTools.Common.Extensions;
    using ProcessingTools.Contracts.Data.Bio.Taxonomy.Repositories;
    using ProcessingTools.Contracts.Data.Repositories;
    using ProcessingTools.Data.Common.Mongo.Contracts;
    using ProcessingTools.Data.Common.Mongo.Factories;
    using ProcessingTools.Enumerations;

    public class BiotaxonomyMongoDatabaseSeeder : IBiotaxonomyMongoDatabaseSeeder
    {
        private readonly IMongoDatabase db;

        private readonly IRepositoryFactory<IMongoTaxonRankRepository> mongoTaxonRankRepositoryFactory;
        private readonly IRepositoryFactory<ITaxonRankRepository> taxonRankRepositoryFactory;

        private readonly IRepositoryFactory<IMongoBiotaxonomicBlackListRepository> mongoBiotaxonomicBlackListRepositoryFactory;
        private readonly IRepositoryFactory<IBiotaxonomicBlackListRepository> biotaxonomicBlackListIterableRepositoryFactory;

        public BiotaxonomyMongoDatabaseSeeder(
            IMongoDatabaseProvider databaseProvider,
            IRepositoryFactory<IMongoTaxonRankRepository> mongoTaxonRankRepositoryFactory,
            IRepositoryFactory<ITaxonRankRepository> taxonRankRepositoryFactory,
            IRepositoryFactory<IMongoBiotaxonomicBlackListRepository> mongoBiotaxonomicBlackListRepositoryFactory,
            IRepositoryFactory<IBiotaxonomicBlackListRepository> biotaxonomicBlackListIterableRepositoryFactory)
        {
            if (databaseProvider == null)
            {
                throw new ArgumentNullException(nameof(databaseProvider));
            }

            this.db = databaseProvider.Create();
            this.mongoTaxonRankRepositoryFactory = mongoTaxonRankRepositoryFactory ?? throw new ArgumentNullException(nameof(mongoTaxonRankRepositoryFactory));
            this.taxonRankRepositoryFactory = taxonRankRepositoryFactory ?? throw new ArgumentNullException(nameof(taxonRankRepositoryFactory));
            this.mongoBiotaxonomicBlackListRepositoryFactory = mongoBiotaxonomicBlackListRepositoryFactory ?? throw new ArgumentNullException(nameof(mongoBiotaxonomicBlackListRepositoryFactory));
            this.biotaxonomicBlackListIterableRepositoryFactory = biotaxonomicBlackListIterableRepositoryFactory ?? throw new ArgumentNullException(nameof(biotaxonomicBlackListIterableRepositoryFactory));
        }

        public async Task<object> Seed()
        {
            await this.SeedTaxonRankTypeCollection();
            await this.SeedTaxonRankCollection();
            await this.SeedBlackListCollection();

            return true;
        }

        private async Task SeedTaxonRankCollection()
        {
            var mongoTaxonRankRepository = this.mongoTaxonRankRepositoryFactory.Create();

            var repository = this.taxonRankRepositoryFactory.Create();
            var query = repository.Query;

            foreach (var entity in query)
            {
                await mongoTaxonRankRepository.Add(entity);
            }

            repository.TryDispose();
            mongoTaxonRankRepository.TryDispose();
        }

        private async Task SeedTaxonRankTypeCollection()
        {
            string collectionName = CollectionNameFactory.Create<MongoTaxonRankTypeEntity>();
            var collection = this.db.GetCollection<MongoTaxonRankTypeEntity>(collectionName);

            var entities = Enum.GetValues(typeof(TaxonRankType)).Cast<TaxonRankType>()
                .Select(rank => new MongoTaxonRankTypeEntity
                {
                    RankType = rank
                })
                .ToList();

            var options = new InsertManyOptions
            {
                IsOrdered = false,
                BypassDocumentValidation = false
            };

            await collection.InsertManyAsync(entities, options);
        }

        private async Task SeedBlackListCollection()
        {
            var mongoBiotaxonomicBlackListRepository = this.mongoBiotaxonomicBlackListRepositoryFactory.Create();
            var repository = this.biotaxonomicBlackListIterableRepositoryFactory.Create();

            var entities = repository.Entities;
            foreach (var entity in entities)
            {
                await mongoBiotaxonomicBlackListRepository.Add(entity);
            }

            repository.TryDispose();
            mongoBiotaxonomicBlackListRepository.TryDispose();
        }
    }
}
