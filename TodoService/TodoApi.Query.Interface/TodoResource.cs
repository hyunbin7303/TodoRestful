using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApi.Query.Interface
{
    public class TodoResource<T> : BaseResponse<T>
    {
        public TodoResource(T todo) : base(todo) { }
        public TodoResource(string msg) : base(msg) { }

    }
}
