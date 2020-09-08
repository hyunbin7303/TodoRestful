
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
        Stopped= 5
    }
}
