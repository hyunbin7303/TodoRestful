using System;
using System.Collections.Generic;
using TodoApi.Model.Todo;

namespace TodoApi.Query.Interface.DTOs
{
    public class UpdateTodoDTO
    {
        public string UserId { get; set; }
        public string Goal { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public TodoStatus Status { get; set; }
        public TodoType TodoType { get; set; }
        public IList<TodoTask> TodoTask { get; set; } = new List<TodoTask>();
        public IList<string> Tags { get; set; } = new List<string>();
        public DateTime Datetime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? ExpectedAmountOfTime { get; set; }
    }
}
