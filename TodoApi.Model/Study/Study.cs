using System;
namespace TodoApi.Model.Study
{
    public class Study
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Goal { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? ExpectedEndTime { get; set; }
        public DateTime? AmountOfTime { get; set; }
        public string Location { get; set; }
        public StudyCategory Studycategory { get; set; }
    }
}
