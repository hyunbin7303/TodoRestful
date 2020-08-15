using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApi.Model.Todo.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(Exception innerException)
            : base("Record cannot be found. ", innerException)
        {

        }
    }
}
