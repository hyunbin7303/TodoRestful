using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Datasource
{
    public class DataAccessorMongo : Base
    {
        public DataAccessorMongo(string _connectionString) : base (_connectionString)
        {

        }

        public List<String> GetDatabase()
        {
            var password = Environment.GetEnvironmentVariable("MongoDB_Password");
            var dbname = "sample+airbnb";
            MongoClientSettings settings = new MongoClientSettings();

            var client = new MongoClient($"mongodb+srv://TodoUser:{password}@workout.oylnh.azure.mongodb.net/{dbname}?retryWrites=true&w=majority");
            var database = client.GetDatabase("test");
            var cursor = client.ListDatabases().ToList();

            var collection = database.GetCollection<object>("listingsAndReviews");
            List<string> dbList = new List<string>();
            cursor.ForEach(db => dbList.Add(db["name"].AsString));

            return dbList;
        }
    }
}
