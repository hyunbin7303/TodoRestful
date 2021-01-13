
using System.ComponentModel;

namespace TodoApi.Model.Todo
{
    public enum TodoStatus : byte 
    {
        [Description("Plan")]
        Plan =1,

        [Description("Progress")]
        Progress = 2,

        [Description("Completed")]
        Completed= 3,

        [Description("Postpone")]
        Postpone= 4,

        [Description("Stopped")]
        Stopped= 5,

        [Description("Unknown")]
        Unknown = 6

    }

    public static class SampleTodoStatus
    {
        public static string Plan = "Plan";
        public static string Start = "Start";
        public static string Progress = "Progress";
        public static string Completed = "Completed";
        public static string Postpone = "Postpone";
        public static string Stopped = "Stopped";
    }

}
