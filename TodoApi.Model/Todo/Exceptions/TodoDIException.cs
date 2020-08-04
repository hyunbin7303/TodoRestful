using System;

namespace TodoApi.Model.Todo.Exceptions
{
    //Workout Dependency injection
    public class TodoDIException :Exception
    {
        public TodoDIException(Exception innerException)
    : base("Service dependency error occurred, contact support", innerException)
        {

        }
    }
}
