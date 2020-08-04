using System;

namespace TodoApi.Model.Todo.Exceptions
{
    public class TodoValidationException : Exception
    {
        public TodoValidationException(Exception innerException)
            : base("Todo Validation exception occured. ", innerException)
        {

        }
    }
}
