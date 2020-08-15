using MongoDB.Bson;
using System;

namespace TodoApi.Datasource
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt => Id.CreationTime;
        public string UserId { get; set; }
        public string Goal { get; set; }
        public string Description { get; set; }
        public DateTime Datetime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? ExpectedAmountOfTime { get; set; }
        public Document()
        {
            this.Datetime = DateTime.Now;
        }
    }
}
