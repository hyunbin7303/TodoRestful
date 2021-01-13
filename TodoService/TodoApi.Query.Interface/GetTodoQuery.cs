using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TodoApi.Model.Todo;

namespace TodoApi.Query.Interface
{
    public class GetTodoQuery
    {
        [Required]
        public string UserId { get; set; }
        public string TodoId { get; set; }
        public bool SortByDate { get; set; }
        public TodoType TodoType { get; set; }
        public TodoStatus TodoStatus { get; set; } 
        public DateTime? Date { get; set; } 
    }
}
