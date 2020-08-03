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

        // Should add filter for username...
        public static Expression<Func<T, bool>> GetByDate<T>(string userId, DateTime? date)
        {

            ParameterExpression paramUser = Expression.Parameter(typeof(T), "u");
            var userProperty = Expression.Property(paramUser, "UserId");
            ConstantExpression userConstant = Expression.Constant(userId, typeof(string));
            Expression UserExpression = Expression.Equal(userProperty, userConstant);

            ParameterExpression param = Expression.Parameter(typeof(T), "d");
            var property = Expression.Property(param, "Datetime");
            ConstantExpression DatetimeConstant = Expression.Constant(date, typeof(DateTime));
            //Expression finalExpression = Expression.LessThanOrEqual(property, DatetimeConstant);
            Expression DateExpression = Expression.Equal(property, DatetimeConstant);


            Expression body = Expression.And(UserExpression, DateExpression);
            var tree = Expression.Lambda<Func<T, bool>>(body, param);
            return tree;
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
            ParameterExpression param = Expression.Parameter(typeof(T), "w");
            var tree = Expression.Lambda<Func<T, bool>>(testExpr, param);
            return tree;
        }
    }
}
