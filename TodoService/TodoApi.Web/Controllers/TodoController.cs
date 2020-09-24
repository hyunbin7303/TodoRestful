using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Serilog;
using TodoApi.Analytics.ExpressionHelper;
using TodoApi.Datasource;
using TodoApi.Model.Todo;
using TodoApi.Model.Todo.Exceptions;
using TodoApi.Query.Interface;
using TodoApi.Web.Services;
using TodoApi.Infrastructure.AutoMapper;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        //https://github.com/HamidMosalla/RestfulApiBestPracticesAspNetCore/tree/master/RestfulApiBestPracticesAspNetCore
        //https://medium.com/@zarkopafilis/asp-net-core-2-2-3-rest-api-28-resource-filtering-67fa61462c31
        //https://github.com/Elfocrash/Youtube.AspNetCoreTutorial/blob/master/Tweetbook/Services/IPostService.cs
        // Can we have Services... rather than calling IMongoRepository directly?
        private readonly IMongoRepository<Todo> _todoRepository;
        private readonly ITodoService _todoService;
        public TodoController(IMongoRepository<Todo> todoRepository, ITodoService todoService)
        {
            _todoRepository = todoRepository ?? throw new ArgumentException(nameof(todoRepository));
            _todoService = todoService ?? throw new ArgumentException(nameof(todoService)); 
        }
        
        [HttpGet("GetAll")]
        public IEnumerable<Todo> GetAll()
        {
            Log.Information("TodoController: Get");
            return _todoRepository.FindAll().Result;
        }

        [HttpGet]
        //[ProducesResponseType(typeof(QueryResultResource<TodoDTO>), 200)]
        public ActionResult<IList<TodoDTO>> Get([FromQuery]GetTodoQuery query)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            try
            {
                var test = _todoService.ListAsync(userId, query).Result;// Testing
                var UserTodoDtos= test.ConvertTo();
                return Ok(UserTodoDtos);
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

        [HttpGet("Todo")]
        public IActionResult GetTodo(Guid todoId)
        {
            Expression<Func<Todo, bool>> todoExpr = null;
            var getTodo = _todoRepository.FindOne(todoExpr);
            if(getTodo == null)
            {
                return NotFound();
            }
            return Ok(getTodo);
        }

        [HttpGet("/todo-completed")]
        public ActionResult<IEnumerable<TodoDTO>> GetTodoCompleted()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            try
            {
                var users = _todoRepository.FindByUserId(userId).Result.ToList();
                var completedTodos = users.FindAll(x => x.Status == TodoStatus.Completed).ConvertTo().ToList();
                return completedTodos;
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

        // Getting Header info.
        [HttpGet("/TestingHeader")]
        public ActionResult<IEnumerable<TodoDTO>> GetHeaderTester([FromHeader]string _header)
        {
            /* Passing Parameter with headers
               Request and Response Body
               Request Authorization
               Response Caching 
               Response Cookies
            */
            return null;
        }




        //https://www.infoworld.com/article/3004496/how-to-work-with-actionresults-in-web-api.html
        //https://code-maze.com/action-filters-aspnetcore/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Todo>> Post([FromBody]Todo todo)
        {
            if (todo == null) return BadRequest();
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            try
            {
                todo.UserId = userId;//TODO Encrypt user Id.
                // How to store subtask?
                todo.TodoTask.Add(new TodoTask { Name = "SubTask Name1", Description = "SubTask Testting1", Progress = TodoStatus.Plan });
                todo.TodoTask.Add(new TodoTask { Name = "SubTask Name2", Description = "SubTask Testting2", Progress = TodoStatus.Plan });
                todo.TodoTask.Add(new TodoTask { Name = "SubTask Name3", Description = "SubTask Testting3", Progress = TodoStatus.Completed });
                todo.TodoTask.Add(new TodoTask { Name = "SubTask Name4", Description = "SubTask Testting3", Progress = TodoStatus.Completed });
                todo.TodoTask.Add(new TodoTask { Name = "SubTask Name5", Description = "SubTask Testting3", Progress = TodoStatus.Completed });
                todo.TodoTask.Add(new TodoTask { Name = "SubTask Name6", Description = "SubTask Testting4", Progress = TodoStatus.Progress });
                todo.TodoTask.Add(new TodoTask { Name = "SubTask Name6", Description = "SubTask Testting4", Progress = TodoStatus.Postpone });
                todo.TodoTask.Add(new TodoTask { Name = "SubTask Name6", Description = "SubTask Testting4", Progress = TodoStatus.Stopped });
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
        [HttpPost("SaveFiles")]
        [AllowAnonymous]
        public IActionResult SaveFile([FromForm] string fileName, [FromForm] IFormFile file)
        {
            string content = String.Empty;
            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    content = reader.ReadToEndAsync().Result;
                }
                var todoObj = JsonConvert.DeserializeObject<List<Todo>>(content);

                //convert todoObj to IDocument, then insert
                
                //_todoRepository.InsertMany();
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return new BadRequestResult();
            }

            //Save files for Todo? Such as json file?
            Console.WriteLine($"Got a file with name: {fileName} and size: {file.Length}");

            return new AcceptedResult();
        }
        //Use PUT when you can update a resource completely through a specific resource. 
        //As soon as you know the new resource location, you can use PUT again to do updates to the blue stapler article
        [HttpPut("{id}")]
        public ActionResult<Task<TodoDTO>> Put(string id, [FromBody]Todo todo)
        {
            if(id != todo.Id.ToString())
            {
                return BadRequest();
            }
            try
            {
                _todoRepository.ReplaceOne(todo);
            }
            catch (TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            catch (TodoDIException diEx)
            {
                return Problem(diEx.Message);
            }
            catch (RecordUpdateConcurrencyException rcEx)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpPut("Subtask/{id}")]
        //public ActionResult<Task<TodoDTO>> updateSubtask(string id)

        //https://medium.com/net-core/how-to-build-a-restful-api-with-asp-net-core-fb7dd8d3e5e3


        [HttpDelete]
        public void Delete(string id)
        {
            // find user info.
            // Delete specific todo request.
            _todoRepository.DeleteById(id);
        }
        [HttpGet("About")]
        [AllowAnonymous]
        public ContentResult About()
        {
            return Content("An API listing Todos of docs.asp.net.");
        }
    }
}
