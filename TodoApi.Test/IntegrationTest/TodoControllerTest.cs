using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TodoApi.Controllers;
using TodoApi.Datasource;
using TodoApi.Model.Todo;
using TodoApi.Web;
using Xunit;

namespace TodoApi.Test
{
    //Integration tests ensure that an app's components function correctly at a level that
    // includes the app's supporting infrastructure
    // db, file system, network appliances, request-response pipeline.
    //https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1#introduction-to-integration-tests 
    //Read this if u need.
    [CollectionDefinition("Integration Tests")]
    public class TodoControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public TodoControllerTest(WebApplicationFactory<Startup> fac)
        {
            _factory = fac;
        }

        [Theory]
        [InlineData("/api/todo/", "")]
        [InlineData("/api/todo/kevin123", "")]
        public async Task GetHttpRequest(string url, string expectedStatus)
        {
            // arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var responseObj = System.Text.Json.JsonSerializer.Deserialize<ResponseType>(
                                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(expectedStatus, responseObj?.Status);
        }

        [Theory]
        [InlineData("/api/todo/GetOnDate/Kevin1234/2020-08-08")]
        [InlineData("/api/todo/GetOnDate/Kevin1234/2020-08-09")]
        [InlineData("/api/todo/GetOnDate/Kevin1234/2020-08-10")]
        public async Task Get_EndpointsGetOnDateTesting(string url)
        {
            // arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var responseObj = System.Text.Json.JsonSerializer.Deserialize<ResponseType>(
                                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            //Assert.Equal(expectedStatus, responseObj?.Status);
        }




        [Theory]
        [InlineData("/")]

        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetAsync(url);
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/api/todo/")]
        public async Task Post_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            Todo sampleTodo = new Todo();
            sampleTodo.Datetime = DateTime.Now;
            sampleTodo.UserId = "Kevin12345";
            var stringContent = new StringContent(JsonConvert.SerializeObject(sampleTodo), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, stringContent);
            var value = await response.Content.ReadAsStringAsync();
            var check = response.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, check.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, check.StatusCode);
        //    Assert.Equal("/", response.Headers.Location.OriginalString);
        }
        private class ResponseType
        {
            public string Status { get; set; }
        }
    }
}
