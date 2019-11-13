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
        public static IQueryable<T> Search<T>(this IQueryable<T> data, string key, string searchString)
        {
            var property = typeof(T).GetProperty(key, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

            if (property == null)
                throw new ArgumentException($"'{typeof(T).Name}' does not implement a public get property named '{key}'.");

            data = data.Where(d => ((string)property.GetValue(d)) != null).AsQueryable();
            return data.Where(d => ((string)property.GetValue(d)).Contains(searchString)).AsQueryable();
        }
    }
}
