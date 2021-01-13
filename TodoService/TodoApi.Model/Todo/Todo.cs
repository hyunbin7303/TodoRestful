using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TodoApi.Datasource;

namespace TodoApi.Model.Todo
{
    [BsonCollection("todo")]
    public class Todo : Document 
    {
        public string TodoId { get; set; }
        [StringLength(30)]
        public string Title { get; set; }
        [StringLength(30)]
        public string Tag { get; set; }
        public TodoStatus Status { get; set; }
        public TodoType TodoType { get; set; }
        public IList<TodoTask> TodoTask { get; set; } = new List<TodoTask>();
        public IList<string> Tags { get; set; } = new List<string>();
    }
}
