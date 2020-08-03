using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TodoApi.Analytics.ExpressionHelper;
using TodoApi.Datasource;
using TodoApi.Model.Workout;
using TodoApi.Model.Workout.Exceptions;

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
        [HttpGet("{userId}")]
        public IEnumerable<Workout> Get(string userId) => _workoutRepository.FindByUserId(userId).Result;


        [HttpGet("gettoday/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Workout>> GetToday(string userId)
        {
            try
            {
                var tree = ExpressionUtils.GetByDate<Workout>(userId, DateTime.Now);
                var check = await _workoutRepository.FindOneAsync(tree);
                return Ok(check);
            }
            catch (WorkoutValidationException workoutValidationEx) when (workoutValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(workoutValidationEx.InnerException.Message);
            }
            catch (WorkoutDIException diEx)
            {
                return Problem(diEx.Message);
            }
        }

        //Working on this part.
        //https://www.infoworld.com/article/3004496/how-to-work-with-actionresults-in-web-api.html
        [HttpGet("GetLast/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Workout>> GetLastAsync(string userId)
        {
            try
            {
                var tree = ExpressionUtils.GetByDate<Workout>(userId, DateTime.Now);
                // TODO Need to make a filter for getting the last one.
                var check = await _workoutRepository.FindOneAsync(tree);
                return Ok(check);
            }
            catch (WorkoutValidationException workoutValidationEx) when (workoutValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(workoutValidationEx.InnerException.Message);
            }
            catch (WorkoutDIException diEx)
            {
                return Problem(diEx.Message);
            }
        }

        [HttpGet("GetOnDate/{userId}/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Workout>>> GetOnDateAsync(string userId, DateTime date)
        {
            try
            {
                var tree = ExpressionUtils.GetByDate<Workout>(userId, date);
                var workout = await _workoutRepository.FindOneAsync(tree);
                return Ok(workout);
            }
            catch(WorkoutValidationException workoutValidationEx) when (workoutValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(workoutValidationEx.InnerException.Message);
            }
            catch(WorkoutDIException diEx)
            {
                return Problem(diEx.Message);
            }

        }

        [HttpGet]
        public ActionResult<List<Workout>> Get([FromQuery] bool discontinuedOnly = false)
        {
            List<Workout> products = null;

            if (discontinuedOnly)
            {
                //products = _workoutRepository.
            }
            else
            {
                products = _workoutRepository.FindAll().Result.ToList();
            }

            return products;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(Workout workout)
        {
            // Find specific user.
            if (workout != null)
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
