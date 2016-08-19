﻿namespace ProcessingTools.Bio.Taxonomy.Data.Mongo.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Contracts;
    using Models;

    using MongoDB.Driver;

    using ProcessingTools.Bio.Taxonomy.Data.Common.Models.Contracts;
    using ProcessingTools.Bio.Taxonomy.Data.Mongo.Contracts;
    using ProcessingTools.Data.Common.Expressions.Contracts;
    using ProcessingTools.Data.Common.Mongo.Repositories;

    public class MongoBiotaxonomicBlackListRepository : MongoCrudRepository<IBlackListEntity, MongoBlackListEntity>, IMongoBiotaxonomicBlackListRepository
    {
        private readonly UpdateOptions updateOptions;

        public MongoBiotaxonomicBlackListRepository(IBiotaxonomyMongoDatabaseProvider provider)
            : base(provider)
        {
            this.updateOptions = new UpdateOptions
            {
                IsUpsert = true,
                BypassDocumentValidation = false
            };
        }

        public override Task<object> Add(IBlackListEntity entity) => this.Update(entity);

        public override async Task<object> Delete(IBlackListEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = await this.Collection.DeleteOneAsync(t => t.Content == entity.Content);

            return result;
        }

        public override async Task<object> Update(IBlackListEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = await this.Collection.UpdateOneAsync(
                Builders<MongoBlackListEntity>.Filter
                    .Eq(t => t.Content, entity.Content),
                Builders<MongoBlackListEntity>.Update
                    .Set(t => t.Content, entity.Content),
                this.updateOptions);

            return result;
        }

        // TODO : repeated code
        public override async Task<object> Update(object id, IUpdateExpression<IBlackListEntity> update)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (update == null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            var updateQuery = this.ConvertUpdateExpressionToMongoUpdateQuery(update);
            var filter = this.GetFilterById(id);
            var result = await this.Collection.UpdateOneAsync(filter, updateQuery);
            return result;
        }
    }
}
