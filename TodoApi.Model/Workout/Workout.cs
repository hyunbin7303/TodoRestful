using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using TodoApi.Datasource;

namespace TodoApi.Model.Workout
{
    [BsonCollection("workout")]
    public class Workout : Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Goal { get; set; }
        public bool IsClass { get; set; }
        
        public TypeOfWorkout workoutType { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime  { get; set; }
        public DateTime? ExpectedAmountOfWork { get; set; }
        
        
    }
}
