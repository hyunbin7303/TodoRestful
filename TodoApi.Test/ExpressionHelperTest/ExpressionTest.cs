using System;
using System.Collections.Generic;
using System.Text;
using TodoApi.Analytics.ExpressionHelper;
using TodoApi.Model.Workout;
using Xunit;

namespace TodoApi.Test.ExpressionHelperTest
{
    public class ExpressionTest
    {
        public ExpressionTest()
        {

            // sample data.
            Workout workout0 = new Workout() { UserId = "Kevin123", Datetime = DateTime.Now };
            Workout workout1 = new Workout() { UserId = "Kevin123", Datetime = new DateTime(2020, 8, 2) };
            Workout workout2 = new Workout() { UserId = "1234", Datetime = new DateTime(2020, 7, 4) };
            Workout workout3 = new Workout() { UserId = "asdf", Datetime = new DateTime(2020, 6, 4) };
            Workout workout4 = new Workout() { UserId = "aaaa", Datetime = new DateTime(2020, 5, 3) };
            Workout workout5 = new Workout() { UserId = "bbbb", Datetime = new DateTime(2020, 2, 20) };
            List<Workout> workouts = new List<Workout>() { workout0, workout1, workout2, workout3, workout4, workout5};
            var check = ExpressionUtils.GetByDate<Workout>("Kevin123", new DateTime(2020, 8, 2)).Compile();
            //workouts.Find(check);

            Assert.True(true);
        }
    }
}
