using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Controllers;
using TodoApi.Datasource;
using TodoApi.Model.Todo;
using Xunit;

namespace TodoApi.Test.DbTest
{
    public class TodoTest
    {

        private readonly IMongoRepository<Todo> _workoutRepository;
        public TodoTest(IMongoRepository<Todo> workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }
        [Fact]
        public async Task IndexReturnsARedirectToIndexHomeWhenIdIsNull()
        {
            // Arrange
            var controller = new TodoController(_workoutRepository);

            // Act
            var result = controller.GetLast("asd");

            // Assert
            var redirectToActionResult =
                Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }


    }
}
