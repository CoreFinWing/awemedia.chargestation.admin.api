using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using System.Runtime.InteropServices;

namespace Awemedia.Admin.AzureFunctions.Business.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {

        private readonly AwemediaContext _context;

        private readonly DbSet<T> _entities;

        private readonly IErrorHandler _errorHandler;

        public BaseRepository(AwemediaContext context, IErrorHandler errorHandler)
        {
            _context = context;
            _entities = context.Set<T>();
            _errorHandler = errorHandler;
        }
        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }
        public IEnumerable<T> GetAll(params string[] include)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (string item in include)
            {
                query = query.Include(item);
            }
            return query.ToList();
        }
        public T GetById(int id)
        {
            return _entities.Find(id);
        }
        public T GetById(int id, string[] includeNavigational, string[] include)
        {
            var entity = _entities.Find(id);
            if (includeNavigational != null)
            {
                foreach (string item in includeNavigational)
                {
                    _context.Entry(entity).Navigation(item).Load();
                }
            }
            if (include != null)
            {
                foreach (string item in include)
                {
                    _context.Entry(entity).Collection(item).Load();
                }
            }
            return entity;
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> exp)
        {
            return _entities.Where(exp);
        }
        public T Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.EntityNull), "", "Input data is null"));
            _entities.AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Update(T entity, [Optional] string[] excludedProps)
        {
            if (entity == null) throw new ArgumentNullException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.EntityNull), "", "Input data is null"));

            _context.Entry(entity).State = EntityState.Modified;
            if (excludedProps != null)
            {
                if (excludedProps.Any())
                {
                    foreach (var name in excludedProps)
                    {
                        _context.Entry(entity).Property(name).IsModified = false;
                    }
                }
            }

            _context.SaveChanges();
        }
        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.EntityNull), "", "Input data is null"));

            _entities.Remove(entity);
            _context.SaveChanges();
        }
        public bool InsertBulk(IEnumerable<T> entities)
        {
            bool result = false;
            if (entities.Count() > 0)
            {
                _entities.AddRange(entities);
                _context.SaveChanges();
                result = true;
            }
            return result;
        }
        public T GetById(Guid guid)
        {
            return _entities.Find(guid);
        }

    }
}
