using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly IMongoRepository<Todo> _todoRepository;
        public TodoController(IMongoRepository<Todo> todoRepository)
        {
            _todoRepository = todoRepository;
        }

        // Going to be deleted. Or Only allow for Admin Claims.
        [HttpGet("GetAll")]
        public IEnumerable<Todo> GetAll()
        {
            Log.Information("TodoController: Get");
            return _todoRepository.FindAll().Result;
        }

        [HttpGet("success")]
        public string Success()
        {
            Log.Information("TodoController: Successful Login");
            return "Successfully Logged in";
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoDTO>> Get([FromQuery]bool sortByDate = false)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            Log.Information($"TodoController: Get {userId}");
            try
            {
                var getUserInfo = _todoRepository.FindByUserId(userId).Result.ConvertTo();
                if (sortByDate)
                {
                    getUserInfo = getUserInfo.OrderByDescending(d => d.Datetime);
                }
                return Ok(getUserInfo);
            }
            catch (TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            catch (TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is RecordNotFoundException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            catch (TodoDIException diEx)
            {
                return Problem(diEx.Message);
            }
        }

        [HttpGet("GetByTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<TodoDTO>> GetByTypes([FromQuery]bool sortByDate = false, [FromQuery]TodoType todoType=TodoType.Others)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                var userTodo = _todoRepository.FindByUserIdAsync(userId).Result.ConvertTo();
                if(sortByDate)
                {
                    userTodo = userTodo.OrderByDescending(d => d.Datetime);
                }
                if(todoType != TodoType.Others)
                {
                    userTodo = userTodo.Where(t => t.TodoType == todoType);
                }
                return Ok(userTodo);
            }
            catch (TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            catch(TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is RecordNotFoundException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            catch (TodoDIException diEx)
            {
                return Problem(diEx.Message);
            }
        }

        /* Passing Parameter with headers
            Request and Response Body
            Request Authorization
            Response Caching 
            Response Cookies
         */
        //https://www.infoworld.com/article/3004496/how-to-work-with-actionresults-in-web-api.html
        [HttpGet("GetOnDate/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<TodoDTO>> GetOnDate(DateTime date)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                var users = _todoRepository.FindByUserId(userId).Result.ToList(); // What if there are like 1000 todos? 
                DateTime ss = new DateTime(date.Year, date.Month, date.Day);
                var getDate = users.Find(x => x.Datetime == ss).ConvertTo();
                return Ok(getDate);
            }
            catch(TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            catch(TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is RecordNotFoundException)
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


        //Sending content in a form is not very common, but it is the best solution if you want to upload a file. Let’s have a look at the example:
        //When sending a request we need to set Content-Type to application/x-www-form-urlencoded and it the Body part, we need to choose a file:
        // Check here: https://www.michalbialecki.com/2020/01/10/net-core-pass-parameters-to-actions/
        [HttpPost]
        public IActionResult SaveFile([FromForm] string fileName, [FromForm] IFormFile file)
        {
            Console.WriteLine($"Got a file with name: {fileName} and size: {file.Length}");
            return new AcceptedResult();
        }


        //Use PUT when you can update a resource completely through a specific resource. 
        //As soon as you know the new resource location, you can use PUT again to do updates to the blue stapler article
        [HttpPut("{id}")]
        public void Put(string UserId, [FromBody] Todo value)
        {
            _todoRepository.ReplaceOne(value);
        }

        [HttpDelete]
        public void Delete(string id)
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
