using System;
using System.Collections.Generic;
using System.Text;
using TodoApi.Model.Todo;

namespace TodoApi.Query.Interface
{
    public class GetTodoQuery
    {
        public bool SortByDate { get; set; }
        public string TodoType { get; set; }
        public string TodoStatus { get; set; } 
        public DateTime? Date { get; set; } 
    }
}
