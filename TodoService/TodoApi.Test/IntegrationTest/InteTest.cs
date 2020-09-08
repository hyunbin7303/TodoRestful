using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TodoApi.Test.IntegrationTest
{
    public class InteTest : IDisposable
    {
        protected readonly HttpClient testClient;
        private readonly IServiceProvider _serviceProvider;

        protected InteTest()
        { 
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
