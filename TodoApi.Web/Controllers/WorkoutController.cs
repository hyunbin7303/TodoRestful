using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        //[HttpGet("{userId:string}", Name = "Get")]
        [HttpGet]
        public IEnumerable<Workout> Get(string userId) => _workoutRepository.FindByUserId(userId).Result;

        // GET: api/Workout/5
        [HttpGet("{UserId}")]
        public ActionResult<Workout> GetLastDate(string UserId)
        {
            Expression<Func<Workout, bool>> test = t => t.Datetime.Contains("aaa");
            var check = _workoutRepository.FindOne(test);
            return Ok(check);
        }


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
