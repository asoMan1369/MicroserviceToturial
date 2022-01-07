using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Repositories
{
    public class ItemsRepository
    {
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> mongoCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public ItemsRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017"); ;
            var database = mongoClient.GetDatabase("Catalog");
            mongoCollection = database.GetCollection<Item>(collectionName);

        }


        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await mongoCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(entity=>entity.Id,id);
            return await mongoCollection.Find(filter).FirstOrDefaultAsync();
        }


        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));

            }

            await mongoCollection.InsertOneAsync(entity); 
        }

        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));

            }

            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, entity.Id);
            await mongoCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);
            await mongoCollection.DeleteOneAsync(filter);
        }
    }
}
