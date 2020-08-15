using System;
using System.Collections.Generic;
using System.Text;
using TodoApi.Analytics.ExpressionHelper;
using TodoApi.Model.Todo;
using Xunit;

namespace TodoApi.Test.ExpressionHelperTest
{
    public class ExpressionTest
    {
        public ExpressionTest()
        {

            // sample data.
            Todo todo0 = new Todo() { UserId = "Kevin123", Datetime = DateTime.Now };
            Todo todo1 = new Todo() { UserId = "Kevin123", Datetime = new DateTime(2020, 8, 2) };
            Todo todo2 = new Todo() { UserId = "1234", Datetime = new DateTime(2020, 7, 4) };
            Todo todo3 = new Todo() { UserId = "asdf", Datetime = new DateTime(2020, 6, 4) };
            Todo todo4 = new Todo() { UserId = "aaaa", Datetime = new DateTime(2020, 5, 3) };
            Todo todo5 = new Todo() { UserId = "bbbb", Datetime = new DateTime(2020, 2, 20) };
            List<Todo> workouts = new List<Todo>() { todo0, todo1, todo2, todo3, todo4, todo5};
            //var check = ExpressionUtils.GetByDate<Todo>("Kevin123", new DateTime(2020, 8, 2)).Compile();
            //workouts.Find(check);

            Assert.True(true);
        }
    }
}
