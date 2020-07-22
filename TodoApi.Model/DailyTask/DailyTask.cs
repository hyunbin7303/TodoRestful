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
        public Chores.Chores chore { get; set; }
        public Jobs.Jobs jobs { get; set; }
    }
}
