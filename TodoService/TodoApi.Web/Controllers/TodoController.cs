﻿using System;
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
using TodoApi.Query.Interface.DTOs;
using TodoApi.Web.Services;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TodoController : ControllerBase
    {
        //https://github.com/HamidMosalla/RestfulApiBestPracticesAspNetCore/tree/master/RestfulApiBestPracticesAspNetCore
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
                var todos = _todoService.ListTodoAsync(GetUserId(), query)?.Result;// Testing
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
                return NotFound();
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
        [HttpPost("SaveFiles")]
        public IActionResult SaveFile([FromForm] string fileName, [FromForm] IFormFile file)
        {
            //Save files for Todo? Such as json file?
            // Check here: https://www.michalbialecki.com/2020/01/10/net-core-pass-parameters-to-actions/
            Console.WriteLine($"Got a file with name: {fileName} and size: {file.Length}");
            return new AcceptedResult();
        }
        
        [HttpPut]
        public ActionResult<Task<TodoDTO>> Put([FromQuery] string TodoId, [FromBody]UpdateTodoDTO todo)
        {
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
            return NoContent();
        }
        [HttpPut("UpdateTodoTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Task<UpdateTodoTaskDTO>> UpdateTodoTask([FromQuery]string TodoId, [FromBody] UpdateTodoTaskDTO subTodoTaskDTO)
        {
            try
            {
                _todoService.UpdateSubTodoAsync(TodoId, subTodoTaskDTO);
            }
            catch(TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            return NoContent();
        }
        [HttpPut("CreateTodoTask")]
        public ActionResult<Task<CreateTodoTaskDTO>> CreateTodoTask([FromQuery] string TodoId, [FromBody]CreateTodoTaskDTO createTodoTaskDTO)
        {
            try
            {
                _todoService.CreateTodoTaskAsync(TodoId, createTodoTaskDTO);
            }
            catch (TodoValidationException todoValidationEx) when (todoValidationEx.InnerException is NotFoundUserException)
            {
                return NotFound(todoValidationEx.InnerException.Message);
            }
            return NoContent();
        }

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
