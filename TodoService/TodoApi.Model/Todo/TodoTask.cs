using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Model.Todo
{
    public class TodoTask
    {
        [Required]
        public string TodoTaskId { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Score { get; set; }
        public DateTime? StartTime { get; set; } = new DateTime();
        public DateTime? EndTime { get; set; } = new DateTime();
        public TodoStatus Progress { get; set; }
    }
}
