using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApi.Datasource
{
    public interface IMongoSettings
    {
        string DatabaseName { get; set; }
        string Connection { get; set; }
    }
}
