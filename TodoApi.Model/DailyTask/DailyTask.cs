using System;
using System.Collections.Generic;
using TodoApi.Datasource;
using System.Text;
using TodoApi.Model.Chores;

namespace TodoApi.Model.DailyTask
{
    [BsonCollection("DailyTask")]
    public class DailyTask : Document
    {
        public List<string> ChoreId { get; set; }
        public List<string> JobId { get; set; }
        public List<string> StudyId { get; set; }
        public List<string> ItemId { get; set; }
        public List<string> WorkoutId { get; set; }
        public DateTime? Today { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
