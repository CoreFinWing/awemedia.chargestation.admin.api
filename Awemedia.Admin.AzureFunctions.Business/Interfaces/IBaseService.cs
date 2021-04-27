using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params string[] includes);
        IQueryable<T> GetTopNRecords(int count, Expression<Func<T, int>> orderBy, params string[] includes);
        T GetById(int id);
        T GetById(int id, [Optional]string[] includeNavigational, [Optional] string[] includes);
        T GetById(Guid guid);
        IEnumerable<T> Where(Expression<Func<T, bool>> exp);
        IQueryable<T> Where(Expression<Func<T, bool>> exp, string[] include);
        T AddOrUpdate(T entry, int Id);
        T AddOrUpdate(T entry, Guid guid, [Optional]string[] excludedProps);
        T AddOrUpdate(T entry, int Id, [Optional]string[] excludedProps);
        void Remove(int id);
        bool InsertBulk(IEnumerable<T> entity);
        IEnumerable<T> Get(out int count,
           Expression<Func<T, bool>> filter = null,
           string[] includePaths = null,
           int? page = null,
           int? pageSize = null);
    }
}
