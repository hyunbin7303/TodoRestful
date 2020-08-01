using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Controllers;
using TodoApi.Datasource;
using TodoApi.Model.Workout;
using Xunit;

namespace TodoApi.Test.DbTest
{
    public class WorkoutTest
    {

        private readonly IMongoRepository<Workout> _workoutRepository;
        public WorkoutTest(IMongoRepository<Workout> workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }
        [Fact]
        public async Task IndexReturnsARedirectToIndexHomeWhenIdIsNull()
        {
            // Arrange
            var controller = new WorkoutController(_workoutRepository);

            // Act
            var result = controller.GetLastAsync("asd");

            // Assert
            var redirectToActionResult =
                Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }


    }
}
