using System;


namespace TodoApi.Model.Todo.Exceptions
{
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException(string userId)
        : base($"Could not find user with ID: {userId}") { }
    }

}
