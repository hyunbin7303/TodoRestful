using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using TodoApi.Datasource;

namespace TodoApi.Model.Workout
{
    [BsonCollection("workout")]
    public class Workout : Document
    {
        [StringLength(30)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Goal { get; set; }
        public WorkoutStatus Status { get; set; }
        public TypeOfWorkout workoutType { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime  { get; set; }
        public TimeSpan? ExpectedAmountOfWork { get; set; }
    }
}
