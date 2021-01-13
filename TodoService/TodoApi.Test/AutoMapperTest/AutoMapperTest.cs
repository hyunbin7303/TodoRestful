using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using TodoApi.Analytics.ExpressionHelper;
using TodoApi.Model.Todo;
using TodoApi.Query.Interface;
using TodoApi.Web;
using Xunit;

namespace TodoApi.Test.AutoMapperTest
{
    using AutoMapper;
    using TodoApi.Infrastructure.Mappings;
    using TodoApi.Query.Interface.DTOs;

    public class AutoMapperTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        //private readonly IMapper _mapper;

        public AutoMapperTest(WebApplicationFactory<Startup> fac)
        {
            _factory = fac;
        }
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<TodoMapping>());
            config.AssertConfigurationIsValid();


            
        }
        [Fact]
        public void Automapper_CreateTodoDTOtoTodo()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<TodoMapping>());
            config.AssertConfigurationIsValid();
            var mapper = new Mapper(config);

            CreateTodoDTO todoDTO = new CreateTodoDTO();
            var mappingTest = mapper.Map<CreateTodoDTO, Todo>(todoDTO);

        }
    }
}
