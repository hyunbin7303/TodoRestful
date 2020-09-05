using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApi.Query.Interface
{
    public class QueryResultResource<T>
    {
        public int TotalTodoItems { get; set; } = 0;
        public List<T> Items { get; set; } = new List<T>();
    }
}
