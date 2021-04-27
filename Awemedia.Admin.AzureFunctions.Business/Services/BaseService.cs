using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class BaseService<T> : IBaseService<T> where T :class
    {
        private readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }
        public IEnumerable<T> GetAll(params string[] include)
        {
            return _repository.GetAll(include);
        }
        public IQueryable<T> GetTopNRecords(int count, Expression<Func<T,int>> orderBy, params string[] includes)
        {
            return _repository.GetTopNRecords(count,orderBy,includes);
        }
        public T GetById(int id)
        {
            return _repository.GetById(id);
        }
        public T GetById(int id, string[] navigationalProperties, string[] include)
        {
            return _repository.GetById(id, navigationalProperties, include);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> exp)
        {
            return _repository.Where(exp);
        }
        public IQueryable<T> Where(Expression<Func<T, bool>> exp, string[] include)
        {
            return _repository.Where(exp, include).AsQueryable();
        }

        public T AddOrUpdate(T entry, int Id)
        {
            var targetRecord = _repository.GetById(Id);
            var exists = targetRecord != null;

            if (exists)
            {
                _repository.Update(entry);
                return entry;
            }
            _repository.Insert(entry);
            return entry;
        }
        public T AddOrUpdate(T entry, int Id, [Optional] string[] excludedProps)
        {
            var targetRecord = _repository.GetById(Id);
            var exists = targetRecord != null;

            if (exists)
            {
                _repository.Update(entry, excludedProps);
                return entry;
            }
            _repository.Insert(entry);
            return entry;
        }
        public T AddOrUpdate(T entry, Guid guid, [Optional] string[] excludedProps)
        {
            var targetRecord = _repository.GetById(guid);
            var exists = targetRecord != null;

            if (exists)
            {
                _repository.Update(entry, excludedProps);
                return entry;
            }
            _repository.Insert(entry);
            return entry;
        }
        public void Remove(int id)
        {
            var label = _repository.GetById(id);
            _repository.Delete(label);
        }

        public bool InsertBulk(IEnumerable<T> entities)
        {
            return _repository.InsertBulk(entities);
        }

        public T GetById(Guid guid)
        {
            return _repository.GetById(guid);
        }
        public IEnumerable<T> Get(out int count, Expression<Func<T, bool>> filter = null, string[] includePaths = null, int? page = null, int? pageSize = null)
        {
            return _repository.Get(out count, filter, includePaths, page, pageSize);
        }
    }
}
