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
        public IEnumerable<Workout> Get() => _workoutRepository.FindAll().Result;

        // GET: api/Workout/5
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<Workout> Get(string userId) => _workoutRepository.FindByUserId(userId).Result;

        [HttpGet("{id}", Name = "GetOnDate")]
        public IEnumerable<Workout> GetOnDate(string userId, string date)
        {
            //_workoutRepository.FilterBy()

            var workouts= _workoutRepository.FindByUserIdandDate(userId, date);
            if(workouts !=null)
            {

                return workouts.Result;
            }
            else
            {
                // Should return with message.
                return null;
            }
        }

        // POST: api/Workout
        //[HttpPost]
        //public void Post(string userId)
        //{
        //    Workout workout = new Workout
        //    {
        //        Id = ObjectId.GenerateNewId(),
        //        UserId = userId,
        //        Status = WorkoutStatus.Plan,
        //        Description = "Testing purpose",
        //        workoutType = TypeOfWorkout.Athletics,
        //        ExpectedAmountOfWork = new TimeSpan(2, minutes: 14, 18),
        //        StartTime = DateTime.Now
        //    };
        //    _workoutRepository.InsertOne(workout);
        //}

        [HttpPost]
        public IActionResult Post(Workout workout)
        {
            // Find specific user.
            if(workout != null)
            {
                _workoutRepository.InsertOne(workout);
                return Ok(workout);
            }
            else
            {
                return BadRequest();
            }
        }


        // PUT: api/Workout/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Workout value)
        {
            _workoutRepository.ReplaceOne(value);
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]

        public void Delete(string id)
        {
            _workoutRepository.DeleteById(id);
        }

        [HttpGet("About")]
        public ContentResult About()
        {
            return Content("An API listing Workouts of docs.asp.net.");
        }


    }
}
