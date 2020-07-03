using System;

namespace TodoApi.Model.Workout
{
    public class Workout
    {
        public string Id { get; set; }
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
