
namespace TodoApi.Model.Todo
{
    public class TodoTask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TodoStatus Progress { get; set; }
    }
}
