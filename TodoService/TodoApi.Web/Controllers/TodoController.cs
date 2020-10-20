using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TodoApi.Datasource;
using TodoApi.Infrastructure.Extensions;
using TodoApi.Model.Todo;
using TodoApi.Model.Todo.Exceptions;
using TodoApi.Query.Interface;
using TodoApi.Web.Services;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        //https://github.com/HamidMosalla/RestfulApiBestPracticesAspNetCore/tree/master/RestfulApiBestPracticesAspNetCore
        // 
        private readonly ITodoService _todoService;
        public TodoController(IMongoRepository<Todo> todoRepository, ITodoService todoService)
        {
            _todoService = todoService ?? throw new ArgumentException(nameof(todoService)); 
        }
        
        [HttpGet("GetAll")]
        public IEnumerable<TodoDTO> GetAll()
        {
            return _todoService.ListAll().Result;
        }
        [HttpGet]
        public ActionResult<IList<TodoDTO>> Get([FromQuery]GetTodoQuery query)
        {
            try
            {
                var todos = _todoService.ListTodoAsync(GetUserId(), query).Result;// Testing
                return Ok(todos);
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
        public ActionResult<TodoDTO> GetTodo(string todoId)
        {
            var getTodo = _todoService.GetOne(todoId);
            if(getTodo == null)
            {
                return NotFound();
            }
            return Ok(getTodo); 
        }
      
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TodoDTO> Post([FromBody] CreateTodoDTO todoDTO)
        {
            if (todoDTO == null) return BadRequest(); // Can we Throw TodoValidationException?
            try
            {
                todoDTO.UserId = GetUserId();//TODO: Encrypt user Id.
                var test = _todoService.SaveAsync(todoDTO);
                return Ok(todoDTO);
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

        // Check here: https://www.michalbialecki.com/2020/01/10/net-core-pass-parameters-to-actions/
        [HttpPost("SaveFiles")]
        public IActionResult SaveFile([FromForm] string fileName, [FromForm] IFormFile file)
        {
            //Save files for Todo? Such as json file?
            Console.WriteLine($"Got a file with name: {fileName} and size: {file.Length}");
            return new AcceptedResult();
        }
        //Use PUT when you can update a resource completely through a specific resource. 
        //As soon as you know the new resource location, you can use PUT again to do updates to the blue stapler article
        [HttpPut("{id}")]
        public ActionResult<Task<TodoDTO>> Put(string TodoId, [FromBody]Todo todo)
        {
            if(TodoId != todo.Id.ToString())
            {
                return BadRequest();
            }
            try
            {
                _todoService.UpdateAsync(TodoId, todo);
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
        public async Task<ActionResult<bool>> Delete(string TodoId)
        {
            try
            {
                var check = await _todoService.DeleteAsync(TodoId);
                return Task.FromResult(check).Result;
            }catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting Data.");
            }
        }

        [HttpGet("About")]
        public ContentResult About()
        {
            return Content("An API listing Todos of docs.asp.net.");
        }
        private string GetUserId()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            return claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
