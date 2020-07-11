using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApi.Model.Study
{
    public class StudyTask
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } = null;
    }
}
