using AutoMapper;
using AutoMapper.Configuration;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using TodoApi.Model.Todo;
using TodoApi.Query.Interface;
using TodoApi.Query.Interface.DTOs;

namespace TodoApi.Infrastructure.Extensions
{
    public static class Extension
    {
        public static IEnumerable<TodoDTO> ConvertTo<Todo>(this IEnumerable<Todo> source)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Todo, TodoDTO>();
            });
            IMapper iMapper = config.CreateMapper();

            return source.Select(todo =>iMapper.Map<Todo, TodoDTO>(todo));
        }
        public static TodoDTO ConvertTo(this Todo source)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Todo, TodoDTO>();
            });
            IMapper iMapper = config.CreateMapper();
            return iMapper.Map<Todo, TodoDTO>(source);
        }
        public static IEnumerable<TDestination> ConvertTo<TSource,TDestination>(this IEnumerable<TSource> source)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return source.Select(todo => iMapper.Map<TSource, TDestination>(todo));
        }
        public static TDestination ConvertTo<TSource, TDestination>(this TSource source)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<TSource, TDestination>(source);
        }

    }
}
