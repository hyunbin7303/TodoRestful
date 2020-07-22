using System;
using TodoApi.Datasource;

namespace TodoApi.Model.Chores
{
    [BsonCollection("chores")]
    public class Chores : Document
    {       
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? ExpectedStartTime { get; set; }
        public DateTime? ExpectedEndTime { get; set; }
        public ChoreType choreType { get; set; }
    }
}
