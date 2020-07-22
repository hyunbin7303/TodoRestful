using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TodoApi.Datasource;

namespace TodoApi.Model.Jobs
{
    [BsonCollection("jobs")]
    public class Jobs : Document
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]

        //[DataMember]
  
        public string Title { get; set; }
        public string Description { get; set; }
        public string Goal { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? ExpectedEndTime { get; set; }
        public JobType JobType { get; set; }
    }

    public enum JobType
    {
        Company,
        Personal,
        Family,
        Others
    }
}
