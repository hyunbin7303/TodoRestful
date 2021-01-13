using System;
using TodoApi.Model.Todo;

namespace TodoApi.Query.Interface.DTOs
{
    public class UpdateTodoTaskDTO
    {
        public string TodoTaskId { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Score { get; set; }
        public DateTime? StartTime { get; set; } = new DateTime();
        public DateTime? EndTime { get; set; } = new DateTime();
        public TodoStatus Progress { get; set; }
        public DateTime? TodoTaskCreated { get; set; } = DateTime.Now;
    }
}
