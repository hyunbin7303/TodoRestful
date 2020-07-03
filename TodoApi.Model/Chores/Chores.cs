using System;

namespace TodoApi.Model.Chores
{
    public class Chores
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? ExpectedStartTime { get; set; }
        public DateTime? ExpectedEndTime { get; set; }
        public ChoreType choreType { get; set; }
    }
}
