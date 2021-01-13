using AutoMapper;
using TodoApi.Model.Todo;
using TodoApi.Query.Interface.DTOs;

namespace TodoApi.Infrastructure.Mappings
{
    public class TodoMapping:Profile
    {
        public TodoMapping()
        {
            CreateMap<Todo, TodoDTO>().ReverseMap();
            CreateMap<Todo, CreateTodoDTO>().ReverseMap();
            CreateMap<Todo, UpdateTodoDTO>().ReverseMap();
            CreateMap<Todo, UpdateTodoTaskDTO>().ReverseMap();
        }
    }
}
