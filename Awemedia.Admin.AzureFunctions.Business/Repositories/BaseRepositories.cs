using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;

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
        public T GetById(int id)
        {
            return _entities.Find(id);
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
        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.EntityNull), "", "Input data is null"));

            var oldEntity = _context.Find<T>(entity);
            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
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
