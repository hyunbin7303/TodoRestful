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
        public string Status { get; set; }
        public string TodoType { get; set; }
        public IList<TodoTask> TodoTask { get; set; } = new List<TodoTask>();
    }
}
