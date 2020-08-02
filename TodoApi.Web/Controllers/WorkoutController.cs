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
        //[HttpGet("{userId:string}", Name = "Get")]
        [HttpGet("{userId}")]
        public IEnumerable<Workout> Get(string userId) => _workoutRepository.FindByUserId(userId).Result;


        [HttpGet("gettoday/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Workout> GetToday(string userId)
        {
            ConstantExpression dateExpr = Expression.Constant(DateTime.Today, typeof(DateTime));
            ParameterExpression param = Expression.Parameter(typeof(Workout), "w");
            var property = Expression.Property(param, "Datetime");
            Expression finalExpression = Expression.Equal(property, dateExpr);
            var tree = Expression.Lambda<Func<Workout, bool>>(finalExpression, param);
            var check = _workoutRepository.FindOne(tree);
            if (check != null)
            {
                return Ok(check);
            }
            else
            {
                return NotFound();
            }
        }

        //Working on this part.
        //https://www.infoworld.com/article/3004496/how-to-work-with-actionresults-in-web-api.html
        [HttpGet("GetLast/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Workout>> GetLastAsync(string userId)
        {
            var tree = ExpressionUtils.GetLastOne<Workout>(userId, new Workout(), DateTime.Now);
            var check = await _workoutRepository.FindOneAsync(tree);
            if (check == null)
            {
                return NotFound();
            }
            return Ok(check);
        }



        [HttpGet("GetOnDate/{userId}/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Workout>> GetOnDate(string userId, DateTime date)
        {
            try
            {
                var workouts = _workoutRepository.FindByUserIdandDate(userId, date);
                return Ok(workouts);
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

        //[HttpPost]
        //[Consumes("application/json")]
        //public IActionResult CreateProduct(Workout product)
        //{
        //    return null;
        //}
        //[HttpPost]
        //[Consumes("application/x-www-form-urlencoded")]
        //public IActionResult PostForm([FromForm] IEnumerable<int> values) => Ok(new { Consumes = "application/x-www-form-urlencoded", Values = values });


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
