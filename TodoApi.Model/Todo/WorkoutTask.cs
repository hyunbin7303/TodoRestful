
namespace TodoApi.Model.Todo
{
    public class WorkoutTask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public WorkoutStatus Progress { get; set; }
    }
}
