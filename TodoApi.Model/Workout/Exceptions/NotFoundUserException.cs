using System;


namespace TodoApi.Model.Workout.Exceptions
{
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException(string userId)
        : base($"Could not find user with ID: {userId}") { }
    }

}
