using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace TodoApi.Analytics.ExpressionHelper
{
    public static class ExpressionUtils
    {
        public static Action<TEntity, TProperty> CreateSetter<TEntity, TProperty>(string name) where TEntity : class
        {
            PropertyInfo propertyInfo = typeof(TEntity).GetProperty(name);

            ParameterExpression instance = Expression.Parameter(typeof(TEntity), "instance");
            ParameterExpression propertyValue = Expression.Parameter(typeof(TProperty), "propertyValue");

            var body = Expression.Assign(Expression.Property(instance, name), propertyValue);

            return Expression.Lambda<Action<TEntity, TProperty>>(body, instance, propertyValue).Compile();
        }

        public static Func<TEntity, TProperty> CreateGetter<TEntity, TProperty>(string name) where TEntity : class
        {
            ParameterExpression instance = Expression.Parameter(typeof(TEntity), "instance");

            var body = Expression.Property(instance, name);

            return Expression.Lambda<Func<TEntity, TProperty>>(body, instance).Compile();
        }
        public static IQueryable<TElement> IsDateBetween<TElement>(this IQueryable<TElement> queryable,
            Expression<Func<TElement, DateTime>> fromDate,
            Expression<Func<TElement, DateTime>> toDate, DateTime dateTime)
        {
            var p = fromDate.Parameters.Single();
            Expression memberEx = p;
            Expression fromExpression = Expression.Property(memberEx, (fromDate.Body as MemberExpression).Member.Name);
            Expression toExpression = Expression.Property(memberEx, (toDate.Body as MemberExpression).Member.Name);

            var after = Expression.LessThanOrEqual(fromExpression, Expression.Constant(dateTime, typeof(DateTime)));
            var before = Expression.GreaterThanOrEqual(toExpression, Expression.Constant(dateTime, typeof(DateTime)));

            Expression body = Expression.And(after, before);
            var predicate = Expression.Lambda<Func<TElement, bool>>(body, p);
            return queryable.Where(predicate);
        }

        public static Expression<Func<T, bool>> GetByDate<T>(string userId, DateTime? date)
        {

            ParameterExpression paramUser = Expression.Parameter(typeof(T), "u");
            var userProperty = Expression.Property(paramUser, "UserId");
            ConstantExpression userConstant = Expression.Constant(userId, typeof(string));
            Expression UserExpression = Expression.Equal(userProperty, userConstant);

            ParameterExpression paramDatetime = Expression.Parameter(typeof(T), "u");
            var property = Expression.Property(paramDatetime, "Datetime");
            ConstantExpression DatetimeConstant = Expression.Constant(date, typeof(DateTime));
            Expression finalExpression = Expression.LessThanOrEqual(property, DatetimeConstant);

            var tree = Expression.Lambda<Func<T, bool>>(UserExpression, paramUser);
            var tree2 = Expression.Lambda<Func<T, bool>>(finalExpression, paramDatetime);
            var aaa = AndAlso(tree, tree2);
            return aaa;
        }
        public static Expression<Func<T, bool>> GetByDateTest<T>(string userId, DateTime? date)
        {
            Expression testExpr = Expression.MemberInit(
                Expression.New(typeof(T)),
                new List<MemberBinding>() {
                    Expression.Bind(typeof(T).GetMember("UserId")[0], Expression.Constant(userId)),
                    Expression.Bind(typeof(T).GetMember("Datetime")[0], Expression.Constant(date))
                }
            );
            var TestCondition = Expression.Constant(userId == "Kevin");
            //var ifTrueBlock = WriteLineExpression("String is the same");
            //ConditionalExpression ifThenExpr = Expression.IfThen(TestCondition, ifTrueBlock);

            ConstantExpression DatetimeConstant = Expression.Constant(date, typeof(DateTime));
            Expression finalExpression = Expression.LessThanOrEqual(testExpr, DatetimeConstant);


            ParameterExpression param = Expression.Parameter(typeof(T), "w");
            var tree = Expression.Lambda<Func<T, bool>>(finalExpression, param);
            return tree;
        }
        static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            // need to detect whether they use the same
            // parameter instance; if not, they need fixing
            ParameterExpression param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0]))
            {
                // simple version
                return Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(expr1.Body, expr2.Body), param);
            }
            // otherwise, keep expr1 "as is" and invoke expr2
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body,Expression.Invoke(expr2, param)), param);
        }
    }
}
