using System;
using System.Collections.Generic;
using System.Text;

namespace Datasource
{
    public abstract class Base
    {
        private string _connectionString;

        protected Base(string _connectionString)
        {
            this._connectionString = _connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
