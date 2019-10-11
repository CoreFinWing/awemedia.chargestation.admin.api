using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IBaseRepository<T>
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params string[] includes);
        T GetById(int id);
        T GetById(int id, [Optional]string[] includeNavigational, [Optional] string[] includes);
        T GetById(Guid guid);
        IEnumerable<T> Where(Expression<Func<T, bool>> exp);
        T Insert(T entity);
        bool InsertBulk(IEnumerable<T> entities);
        void Update(T entity, [Optional]string[] excludedProps);
        void Delete(T entity);
    }
}
