using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApi.Model.Todo.Exceptions
{
    public class RecordUpdateConcurrencyException : Exception
    {
        public RecordUpdateConcurrencyException(Exception innerException)
            : base("Datbase error occurred, contact support", innerException)
        {
            
        }
    }
}
