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

            var after = Expression.LessThanOrEqual(fromExpression,Expression.Constant(dateTime, typeof(DateTime)));
            var before = Expression.GreaterThanOrEqual(toExpression, Expression.Constant(dateTime, typeof(DateTime)));

            Expression body = Expression.And(after, before);
            var predicate = Expression.Lambda<Func<TElement, bool>>(body, p);
            return queryable.Where(predicate);
        }

        // Should add filter for username...
        public static Expression<Func<T,bool>> GetLastOne<T>(string userId, T getObj, DateTime? date)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "d");
            var property = Expression.Property(param, "Datetime");
            ConstantExpression DatetimeConstant = Expression.Constant(DateTime.Now, typeof(DateTime));
            Expression finalExpression = Expression.LessThanOrEqual(property, DatetimeConstant);
            var tree = Expression.Lambda<Func<T, bool>>(finalExpression, param);
            return tree;
        }
    }
}
