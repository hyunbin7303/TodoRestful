using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApi.Datasource
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt => Id.CreationTime;
    }
}
