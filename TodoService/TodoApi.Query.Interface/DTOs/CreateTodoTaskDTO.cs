using System;
using System.ComponentModel.DataAnnotations;
using TodoApi.Model.Todo;

namespace TodoApi.Query.Interface.DTOs
{
    public class CreateTodoTaskDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Score { get; set; }
        public DateTime? StartTime { get; set; } = DateTime.Now;
        public DateTime? EndTime { get; set; } = new DateTime();
        public TodoStatus Progress { get; set; }
    }
}
