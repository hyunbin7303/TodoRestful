﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Serilog;
using Serilog.Core;
using TodoApi.Analytics.ExpressionHelper;
using TodoApi.Datasource;
using TodoApi.Model.Todo;
using TodoApi.Model.Todo.Exceptions;
using TodoApi.Query.Interface;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IMongoRepository<Todo> _todoRepository;
        public TodoController(IMongoRepository<Todo> todoRepository)
        {
            _todoRepository = todoRepository;
        }
        [HttpGet]
        public IEnumerable<Todo> Get()
        {
            Log.Information("TodoController: Get");
            return _todoRepository.FindAll().Result;
        }

        // GET: api/Todo/5
        [HttpGet("{userId}")]
        public IEnumerable<TodoDTO> Get(string userId)
        {
            Log.Information($"TodoController: Get {userId}");
            return _todoRepository.FindByUserId(userId).Result.ConvertTo();
        }

        [HttpGet("GetByUser/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<TodoDTO>> GetByUser(string userId)
        {
            try
            {
                var check = _todoRepository.FindByUserIdAsync(userId).Result.ConvertTo();
                return Ok(check);
            }
            catch (TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            catch (TodoDIException diEx)
            {
                return Problem(diEx.Message);
            }
        }


        [HttpGet("gettoday/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TodoDTO> GetToday(string userId)
        {
            try
            {
                var todos =  _todoRepository.FindByUserId(userId).Result.ToList();
                var todoDTOs = todos.FindAll(t => t.Datetime == DateTime.Now).ConvertTo();
                return Ok(todoDTOs);
            }
            catch (TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            catch (TodoDIException diEx)
            {
                return Problem(diEx.Message);
            }
        }

        //Working on this part.
        //https://www.infoworld.com/article/3004496/how-to-work-with-actionresults-in-web-api.html
        [HttpGet("GetLast/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TodoDTO> GetLast(string userId)
        {
            try
            {
                // Get by user first.
                var todos = _todoRepository.FindByUserIdAsync(userId).Result.ConvertTo();
                var lastTodo = todos.OrderByDescending(date => date).First();
                return Ok(lastTodo);
            }
            catch (TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            catch (TodoDIException diEx)
            {
                return Problem(diEx.Message);
            }
        }


        [HttpGet("GetOnDate/{userId}/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<TodoDTO>> GetOnDate(string userId, DateTime date)
        {
            try
            {
                var check = _todoRepository.FindByUserId(userId).Result.ToList();

                DateTime ss = new DateTime(date.Year, date.Month, date.Day);
                var getDate = check.Find(x => x.Datetime == ss).ConvertTo();
                return Ok(getDate);
            }
            catch(TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            catch(TodoDIException diEx)
            {
                return Problem(diEx.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Todo>> Post(Todo todo)
        {
            try
            {
                await _todoRepository.InsertOneAsync(todo);
                return Ok(todo);
            }
            catch (TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            catch (TodoDIException diEx)
            {
                return Problem(diEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Todo value)
        {
            _todoRepository.ReplaceOne(value);
        }

        [HttpDelete("{userId}")]

        public void Delete(string userId, string id)
        {
            // find user info.

            // Delete specific todo request.
            _todoRepository.DeleteById(id);
        }

        [HttpGet("About")]
        public ContentResult About()
        {
            return Content("An API listing Todos of docs.asp.net.");
        }
    }
}
