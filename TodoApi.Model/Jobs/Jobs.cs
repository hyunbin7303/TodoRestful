using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApi.Model.Jobs
{
    class Jobs
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Goal { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? ExpectedEndTime { get; set; }
        //public JobType JobType { get; set; }
    }
}
