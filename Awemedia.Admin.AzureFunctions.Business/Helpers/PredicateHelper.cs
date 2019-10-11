using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Helpers
{
    public class PredicateHelper<T> where T : class
    {
        public static Expression<Func<T, bool>> CreateSearchPredicate(string columnName, object searchValue)
        {
            //var xType = typeof(T);
            //var x = Expression.Parameter(xType, "x");
            //var column = xType.GetProperties().FirstOrDefault(p => p.Name == columnName);
            //MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            //var body = column == null
            //    ? (Expression)Expression.Constant(true)
            //    : Expression.Equal(
            //        Expression.PropertyOrField(x, columnName),
            //        Expression.Constant(searchValue));

            //return Expression.Lambda<Func<T, bool>>(body, x);
            var parameterExp = Expression.Parameter(typeof(T), "type");
            var propertyExp = Expression.Property(parameterExp, columnName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var someValue = Expression.Constant(searchValue, typeof(string));
            var containsMethodExp = Expression.Call(propertyExp, method, someValue);

            return Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
        }
    }
}
