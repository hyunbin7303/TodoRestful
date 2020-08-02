using System;

namespace TodoApi.Model.Workout.Exceptions
{
    public class WorkoutValidationException : Exception
    {
        public WorkoutValidationException(Exception innerException)
            : base("Workout Validation exception occured. ", innerException)
        {

        }
    }
}
