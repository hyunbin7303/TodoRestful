using System;
using System.Collections.Generic;
using System.Text;
using TodoApi.Model.Todo;

namespace TodoApi.Query.Interface
{

    // Currently it looks same with TodoDTO, but we will see.
    public class CreateTodoDTO
    {
        public string UserId { get; set; }
        public string Goal { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public TodoStatus Status { get; set; }
        public TodoType TodoType { get; set; }
        public IList<TodoTask> TodoTask { get; set; } = new List<TodoTask>();
        public DateTime Datetime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? ExpectedAmountOfTime { get; set; }
    }
}
