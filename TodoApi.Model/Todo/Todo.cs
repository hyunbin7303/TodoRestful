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
        public WorkoutStatus Status { get; set; }
        public TypeTodo TodoType { get; set; }
        public IList<WorkoutTask> workoutTask { get; set; }
        
    }
}
