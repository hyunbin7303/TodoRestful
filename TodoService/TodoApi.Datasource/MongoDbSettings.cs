using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApi.Datasource
{
    public class MongoDbSettings : IMongoSettings
    {
        public string DatabaseName { get; set; }
        public string Connection { get; set; }
    }
}
