using NUnit.Framework;
using TodoApi.Datasource;

namespace TodoApi.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DatabaseConnectionTesting()
        {
            DataAccessorMongo mongo = new DataAccessorMongo("");
            var check = mongo.DatabaseConnectionTesting();
            Assert.IsTrue(check);       
        }
        
    }
}