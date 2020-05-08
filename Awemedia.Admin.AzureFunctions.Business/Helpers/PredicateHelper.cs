using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Helpers
{
    public static class PredicateHelper
    {
        public static IEnumerable<T> Search<T>(this IEnumerable<T> data, string key, string searchString)
        {
            var property = typeof(T).GetProperty(key, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

            if (property == null)
                throw new ArgumentException($"'{typeof(T).Name}' does not implement a public get property named '{key}'.");

            data = data.Where(d => property.GetValue(d) != null).ToList();
            return data.Where(d => Convert.ToString(property.GetValue(d)).Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string sortBy, string sortDirection)
        {
            var param = Expression.Parameter(typeof(T), "item");

            var sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

            switch (sortDirection.ToLower())
            {
                case "asc":
                    return source.AsQueryable().OrderBy(sortExpression);
                default:
                    return source.AsQueryable().OrderByDescending(sortExpression);

            }
        }
    }
}
