using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Providers.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Datasource;
using TodoApi.Model;
using TodoApi.Model.DailyTask;

namespace TodoApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyTaskController : ControllerBase
    {
        private readonly IMongoRepository<DailyTask> _repository;

        public DailyTaskController(IMongoRepository<DailyTask> repository)
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<DailyTask> Get()
        {
            var dailyTasks = _repository.FindAll();
            return dailyTasks.Result;
        }
        [HttpGet("{id}")]
        public DailyTask Get(string id)
        {
            return _repository.FindById(id);
        }

        [HttpPost]
        public void Post([FromBody]DailyTask task)
        {
            //validate chore
            _repository.InsertOne(task);
        }


    }
}
