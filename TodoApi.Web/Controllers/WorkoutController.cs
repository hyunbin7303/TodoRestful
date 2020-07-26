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


        [HttpGet("gettoday/{userId}")]
        public ActionResult<Workout> GetToday(string userId)
        {
            //Expression<Func<DateTime, bool>> TodayDate = x => x.Date.CompareTo
            //var dateexpr = Expression.Constant( Convert.ToDateTime(filter.FilterValue).Date.AddDays(1),
            //     typeof(DateTime)
            //   );
            return null;
        }

        //Working on this part.
        //https://www.infoworld.com/article/3004496/how-to-work-with-actionresults-in-web-api.html
        [HttpGet("getlastdate/{userId}/{datetime}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Workout> GetLastdate(string userId, DateTime datetime)
        {
            if(datetime!= null)
            {
                var setterNameProperty = ExpressionUtils.CreateSetter<Workout, string>("UserId");
                var setterDatetimeProperty = ExpressionUtils.CreateSetter<Workout, DateTime>("datetime");

                var Workout = new Workout();
                setterNameProperty(Workout, userId);
                setterDatetimeProperty(Workout, datetime);
                Console.WriteLine("Name: {0}, DOB: {1}", Workout.UserId, Workout.Datetime);
                Expression<Func<Workout, bool>> test = t => t.Datetime.Equals(new DateTime(2019, 5, 2));
                var check = _workoutRepository.FindOne(test);
                if(check ==null)
                {
                    return NotFound();
                }
                return Ok(check);
            }
            else
            {
                return BadRequest();
            }
        }



        [HttpGet("{id}", Name = "GetOnDate")]
        public IEnumerable<Workout> GetOnDate(string userId, DateTime date)
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
        [Consumes("application/json")]
        public IActionResult CreateProduct(Workout product)
        {
            return null;
        }
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult PostForm([FromForm] IEnumerable<int> values) =>
    Ok(new { Consumes = "application/x-www-form-urlencoded", Values = values });




        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
