using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TodoApi.Controllers;
using TodoApi.Datasource;
using TodoApi.Web;
using Xunit;

namespace TodoApi.Test
{
    [CollectionDefinition("Integration Tests")]
    public class WorkoutControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public WorkoutControllerTest(WebApplicationFactory<Startup> fac)
        {
            _factory = fac;
        }

        [Theory]
        [InlineData("/api/Workout/", "")]
        [InlineData("/api/Workout/kevin123", "")]
        public async Task GetHttpRequest(string url, string expectedStatus)
        {
            // arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var responseObj = JsonSerializer.Deserialize<ResponseType>(
                                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(expectedStatus, responseObj?.Status);
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

        private class ResponseType
        {
            public string Status { get; set; }
        }
    }
}
