using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TodoApi.Datasource;
using TodoApi.Model.Workout;

namespace TodoApi.Controllers
{
    //https://thecodebuzz.com/mongodb-repository-implementation-unit-testing-net-core-example/
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IMongoRepository<Workout> _workoutRepository;
        public WorkoutController(IMongoRepository<Workout> workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }
        // GET: api/Workout
        [HttpGet]
        public IEnumerable<Workout> Get()
        {
            var getWorkout = _workoutRepository.FindAll();
            return null;
        }

        // GET: api/Workout/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(string id)
        {
            _workoutRepository.FindByUserId(id);
            return "Success";
        }
        // POST: api/Workout
        [HttpPost]
        public void Post(string userId)
        {
            Workout workout = new Workout
            {
                Id = ObjectId.GenerateNewId(),
                UserId = userId,
                IsClass = false,
                Description = "Testing purpose",
                workoutType = TypeOfWorkout.Athletics,
                ExpectedAmountOfWork = new TimeSpan(2, minutes: 14, 18),
                StartTime = DateTime.Now
            };
            _workoutRepository.InsertOne(workout);
        }
        // PUT: api/Workout/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
