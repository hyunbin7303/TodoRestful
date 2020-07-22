using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Datasource;
using TodoApi.Model.Chores;

namespace TodoApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoresController : ControllerBase
    {
        private readonly IMongoRepository<Chores> _choresRepository;

        public ChoresController(IMongoRepository<Chores> choresRepository)
        {
            _choresRepository = choresRepository;
        }

        [HttpGet]
        public IEnumerable<Chores> Get()
        {
            var getChores = _choresRepository.FindAll();
            return getChores.Result;
        }
        [HttpGet("{id}")]
        public Chores Get(string id)
        {
            var getChores = _choresRepository.FindById(id);
            return getChores;
        }
        [HttpPost]
        public void Post([FromBody]Chores chore)
        {
            //validate chore
            _choresRepository.InsertOne(chore);
        }
    }
}
