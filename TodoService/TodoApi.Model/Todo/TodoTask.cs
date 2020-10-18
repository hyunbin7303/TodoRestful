using System;
namespace TodoApi.Model.Todo
{
    public class TodoTask
    {
        public string TodoTaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Score { get; set; }
        public DateTime? StartTime { get; set; } = new DateTime();
        public DateTime? EndTime { get; set; } = new DateTime();
        public TodoStatus Progress { get; set; }
    }
}
