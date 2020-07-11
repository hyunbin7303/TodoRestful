using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;

namespace TodoApi.Datasource
{
    public class DataAccessorMongo : Base
    {
        public DataAccessorMongo(string _connectionString) : base (_connectionString)
        {

        }

        public List<String> GetDatabase()
        {
            var password = Environment.GetEnvironmentVariable("MongoDB_Password");
            var dbname = "sample_airbnb";
            MongoClientSettings settings = new MongoClientSettings();

            var client = new MongoClient($"mongodb+srv://TodoUser:{password}@workout.oylnh.azure.mongodb.net/{dbname}?retryWrites=true&w=majority");
            var database = client.GetDatabase("test");
            var cursor = client.ListDatabases().ToList();
            

            var collection = database.GetCollection<object>("listingsAndReviews");
            List<string> dbList = new List<string>();
            cursor.ForEach(db => dbList.Add(db["name"].AsString));

            return dbList;
        }
        public IMongoClient GetClient(string connString, string dbName)
        {
            var password = Environment.GetEnvironmentVariable("MongoDB_Password");
            return new MongoClient($"mongodb+srv://TodoUser:{password}@todo.oylnh.azure.mongodb.net/{dbName}?retryWrites=true&w=majority");
        }
        public IMongoDatabase GetDatabase(IMongoClient client)
        {
            return client.GetDatabase("Workout");
        }
        public IMongoCollection<T> MongoCollection<T>(IMongoDatabase mDb, string collectionName)
        {
            return mDb.GetCollection<T>(collectionName);
        }
    }
}
