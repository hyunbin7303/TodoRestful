using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TodoApi.Datasource;

namespace TodoApi.Model.Todo
{
    [BsonCollection("todo")]
    public class Todo : Document 
    {
        [StringLength(30)]
        public string Title { get; set; }
        public TodoStatus Status { get; set; }
        public TodoType TodoType { get; set; }
        public IList<TodoTask> TodoTask { get; set; }
        public Todo()
        {
            TodoTask = new List<TodoTask>();
        }
        
    }
}
