using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Model.Todo;
using TodoApi.Query.Interface;

namespace TodoApi.Infrastructure.Mappings
{
    public class TodoMapping:Profile
    {
        public TodoMapping()
        {
            CreateMap<Todo, TodoDTO>().ReverseMap();
            CreateMap<Todo, CreateTodoDTO>().ReverseMap();

        }
    }
}
