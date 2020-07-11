using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TodoApi.Datasource
{
    public interface IDocument
    {
        ObjectId Id { get; set; }
        DateTime CreatedAt { get; }
    }
}
