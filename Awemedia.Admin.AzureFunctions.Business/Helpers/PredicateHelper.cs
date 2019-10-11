using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Helpers
{
    public class PredicateHelper<T> where T : class
    {
        public static Expression<Func<T, bool>> CreateSearchPredicate(string columnName, object searchValue)
        {
            var xType = typeof(T);
            var x = Expression.Parameter(xType, "x");
            var column = xType.GetProperties().FirstOrDefault(p => p.Name == columnName);

            var body = column == null
                ? (Expression)Expression.Constant(true)
                : Expression.Equal(
                    Expression.PropertyOrField(x, columnName),
                    Expression.Constant(searchValue));

            return Expression.Lambda<Func<T, bool>>(body, x);
        }
    }
}
