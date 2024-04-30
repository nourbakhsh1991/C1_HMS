using BMS.Domain.DataSettings;
using BMS.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Domain.MongoDb
{
    public class MongoDBContext : IDatabaseContext
    {
        protected IMongoDatabase _database;
        private readonly IServiceProvider _serviceProvider;

        public MongoDBContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public MongoDBContext(string connectionString)
        {
            var mongourl = new MongoUrl(connectionString);
            var databaseName = mongourl.DatabaseName;
            _database = new MongoClient(connectionString).GetDatabase(databaseName);
        }

        public MongoDBContext(IMongoDatabase mongodatabase)
        {
            _database = mongodatabase;
        }

        public IMongoDatabase Database()
        {
            return _database;
        }

        public IQueryable<T> Table<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName).AsQueryable();
        }

        protected IMongoDatabase TryReadMongoDatabase()
        {
            (var setting, _) = DataSettingsManager.LoadSettings();

            var mongourl = new MongoUrl(setting.connectionString);
            var databaseName = mongourl.DatabaseName;
            var mongodb = new MongoClient(setting.connectionString).GetDatabase(databaseName);
            return mongodb;
        }

        public async Task<bool> DatabaseExist(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var databaseName = new MongoUrl(connectionString).DatabaseName;
            var database = client.GetDatabase(databaseName);
            await database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");

            var filter = new BsonDocument("name", "DBVersion");
            var found = database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter }).Result;
            if (found.Any())
                return true;
            else
                return false;
        }

        public async Task CreateTable(string name, string collation)
        {
            try
            {
                var database = _database ?? TryReadMongoDatabase();

                if (!string.IsNullOrEmpty(collation))
                {
                    var options = new CreateCollectionOptions();
                    options.Collation = new Collation(collation);
                    await database.CreateCollectionAsync(name, options);
                }
                else
                    await database.CreateCollectionAsync(name);

            }
            catch (System.Exception ex)
            {
                // TODO
            }

        }

        public async Task CreateDatabase()
        {
            try
            {
                (var settings, _) = DataSettingsManager.LoadSettings();

                if (!DataSettingsManager.DatabaseIsInstalled())
                {
                    await DataSettingsManager.SaveSettings(settings);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
