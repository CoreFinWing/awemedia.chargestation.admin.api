using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IBaseService<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T GetById(Guid guid);
        IEnumerable<T> Where(Expression<Func<T, bool>> exp);
        T AddOrUpdate(T entry, int Id);
        T AddOrUpdate(T entry, Guid guid, [Optional]string[] excludedProps);
        void Remove(int id);
        bool InsertBulk(IEnumerable<T> entity);
    }
}
