using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TodoApi.Datasource;
using TodoApi.Model.Jobs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IMongoRepository<Jobs> _jobRepository;
        public JobsController(IMongoRepository<Jobs> jobRepository)
        {
            _jobRepository = jobRepository;
        }
        // GET: api/<JobsController>
        [HttpGet]
        public Task<IList<Jobs>> Get()
        {
            return _jobRepository.FindAll();
        }

        // GET api/<JobsController>/5
        [HttpGet("{id}")]
        public Task<IList<Jobs>> Get(string id)
        {
            return _jobRepository.FindByUserId(id);
        }
        [HttpGet("search/{keyword}")]
        public Task<IList<Jobs>> Search(string keyword)
        {
            return _jobRepository.Search(keyword);
        }

        // POST api/<JobsController>
        [HttpPost]
        public ActionResult<Jobs> Post([FromBody] Jobs value)
        {
            value.Id = ObjectId.GenerateNewId();
            _jobRepository.InsertOne(value);
            return value;
        }

        // PUT api/<JobsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Jobs value)
        {
            return NoContent();
        }

        // DELETE api/<JobsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return NoContent();
        }
    }
}
