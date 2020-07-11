using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Workout/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            DataAccessorMongo mongo = new DataAccessorMongo("");

            mongo.GetDatabase();
            string name = "";
            JsonResult json = new JsonResult(mongo.GetDatabase());
          
            mongo.GetDatabase().ForEach(x => name += $"[name:{x}], ");
            return $"{name}";

        }



        // POST: api/Workout
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
