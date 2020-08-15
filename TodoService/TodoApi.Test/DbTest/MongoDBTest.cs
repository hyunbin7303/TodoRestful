using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using TodoApi.Model.Todo;

namespace TodoApi.Test.DbTest
{
    public class MongoDBTest
    {
        public static void TestBasic()
        {

            MongoClient dbClient = new MongoClient("mongodb+srv://TodoUser:lAoyG54cyz6FS9DK@todo.oylnh.azure.mongodb.net/Todo?retryWrites=true&w=majority&connect=replicaSet");
            var database = dbClient.GetDatabase("Todo");
            var collection = database.GetCollection<BsonDocument>("todo");
            var documents = new[]
            {
                new Todo
                {
                    UserId = "Kevin123",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddDays(2),
                    TodoTask = new List<TodoTask>()

                },
                
            };
            //collection.InsertMany(documents);
        }


    }
}
