using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApi.Model.Workout.Exceptions
{
    //Workout Dependency injection
    public class WorkoutDIException :Exception
    {
        public WorkoutDIException(Exception innerException)
    : base("Service dependency error occurred, contact support", innerException)
        {

        }
    }
}
