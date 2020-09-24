using System;
using System.Collections.Generic;
using System.Text;
using TodoApi.Model.Todo;

namespace TodoApi.Query.Interface
{
    public class GetTodoQuery
    {
        public string UserId { get; set; }
        public bool SortByDate { get; set; }
        public TodoType TodoType { get; set; }
        public TodoStatus TodoStatus { get; set; } 
        public DateTime? Date { get; set; } 
    }
}
