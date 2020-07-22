﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TodoApi.Datasource
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }
        string UserId { get; set; }
        string Datetime { get; set; }
        DateTime CreatedAt { get; }
    }
}
