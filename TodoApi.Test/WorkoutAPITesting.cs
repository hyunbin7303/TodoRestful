using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TodoApi.Datasource;
using TodoApi.Model.Workout;

namespace TodoApi.Test
{
    class WorkoutAPITesting
    {
        [Test]
        public void Workout_InsertOneAsync()
        {
            DataAccessorMongo mongo = new DataAccessorMongo("");
            //var client = mongo.GetClient("", "Todo");
            //var database = client.GetDatabase("Todo");
            //var collection = database.GetCollection<BsonDocument>("Workout");

            var client = new MongoClient("mongodb+srv://TodoUser:lAoyG54cyz6FS9DK@todo.oylnh.azure.mongodb.net/Todo?retryWrites=true&w=majority&connect=replicaSet");
            var databaset = client.GetDatabase("Todo");
            var collection = databaset.GetCollection<Workout>("Workout");

            var workout = new Workout
            {
                UserId = "HAHAH",
                Description= "TEsting"
            };

            collection.InsertOne(workout);
            //await collection.InsertOneAsync(document);
        }
    }
}
