using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApi.Datasource
{
    public interface IMongoSettings
    {
        string DBname { get; set; }
        string ConnectionString { get; set; }
    }
}
