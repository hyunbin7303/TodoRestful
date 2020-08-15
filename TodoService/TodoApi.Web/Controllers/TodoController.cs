using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
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
        public IEnumerable<TodoDTO> Get(string userId, [FromQuery]bool sortByDate = false)
        {
            Log.Information($"TodoController: Get {userId}");
            var getUserInfo = _todoRepository.FindByUserId(userId).Result.ConvertTo();
            if (sortByDate)
            {
                getUserInfo = getUserInfo.OrderByDescending(d => d.Datetime);
            }
            return getUserInfo;
        }
        // Query parameters give you the option to modify your request with key-value pairs.

        // Going to change later.
        [HttpGet("IsUserValid/{userValidation}")]
        public IActionResult IsUserValid([FromHeader] string userValidation)
        {
            
            try
            {
                string regexExpression = "^(?:(?<visa>4[0-9]{12}(?:[0-9]{3})?)|" + "(?<mastercard>5[1-5][0-9]{14})|" + "(?<amex>3[47][0-9]{13})|)$";
                Regex regex = new Regex(regexExpression);
                var match = regex.Match(userValidation);
                // TODO : User validation Checking.
                return Ok(match.Success);
            }
            catch (Exception e)
            {
               // This means the user isn’t not authorized to access a resource. It usually returns when the user isn’t authenticated.
                return Unauthorized(userValidation);
                
                //403 Forbidden – This means the user is authenticated, but it’s not allowed to access a resource.
            }
        }

        [HttpGet("GetByUser/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<TodoDTO>> GetByUser(string userId, [FromQuery]bool sortByDate = false, [FromQuery]TodoType todoType=TodoType.Others)
        {
            try
            {
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
        [HttpGet("GetOnDate/{userId}/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<TodoDTO>> GetOnDate(string userId, DateTime date)
        {
            try
            {
                var users = _todoRepository.FindByUserId(userId).Result.ToList(); // What if there are like 1000 todos? 
                //// Get the recent one.
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

        [HttpGet("Getbytypes/{userId}/{todoType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<TodoDTO>> GetByTypes(string userId, [FromQuery]TodoType todoType=TodoType.Others)
        {
            try
            {
                var check = _todoRepository.FindByUserIdAsync(userId).Result.ToList();
                var checkTypes = check.FindAll(x => x.TodoType.Equals(todoType));
                return Ok(checkTypes);
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
            // User Authentication
            //UserId
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
