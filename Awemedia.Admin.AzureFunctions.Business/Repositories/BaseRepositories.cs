﻿using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using System.Runtime.InteropServices;
using System.Reflection;

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
        public IQueryable<T> GetAll()
        {
            return _entities.AsQueryable();
        }
        public IQueryable<T> GetAll(params string[] include)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (string item in include)
            {
                query = query.Include(item);
            }
            return query.AsQueryable();
        }
        public IQueryable<T> GetTopNRecords(int count, Expression<Func<T,int>> orderBy,params string[] include)
        {
            IQueryable<T> query = _context.Set<T>().OrderByDescending(orderBy).Take(count);
            foreach (string item in include)
            {
                query = query.Include(item);
            }
            return query.AsQueryable();
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
            return _entities.Where(exp).AsQueryable();
        }
        public IQueryable<T> Where(Expression<Func<T, bool>> exp, string[] include)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (string item in include)
            {
                query = query.Include(item);
            }
            return query.Where(exp).AsQueryable();
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

            try
            {
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
            catch (Exception ex)
            {
                if (!ex.Message.Contains("Database operation expected to affect 1 row(s) but actually affected 0 row(s)"))
                    throw;
            }
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

        public IEnumerable<T> Get(out int count,
           Expression<Func<T, bool>> filter = null,
           string[] includePaths = null,
           int? page = null,
           int? pageSize = null)
        {
            IQueryable<T> query = _entities;
            count = query.Count();
            if (includePaths != null)
            {
                for (var i = 0; i < includePaths.Count(); i++)
                {
                    query = query.Include(includePaths[i]);
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
                count = query.Count();
            }
            if (pageSize != null)
            {
                query = query.Skip(((int)page - 1) * (int)pageSize).Take((int)pageSize);
            }
            return query.ToList();
        }

        public IEnumerable<T> Get(out int count,
           Expression<Func<T, int>> filter = null,
           string[] includePaths = null,
           int? page = null,
           int? pageSize = null)
        {
            IQueryable<T> query = _entities;
            count = query.Count();
            if (includePaths != null)
            {
                for (var i = 0; i < includePaths.Count(); i++)
                {
                    query = query.Include(includePaths[i]);
                }
            }
            if (filter != null)
            {
                query = query.OrderByDescending(filter);
                count = query.Count();
            }
            if (pageSize != null)
            {
                query = query.Skip(((int)page - 1) * (int)pageSize).Take((int)pageSize);
            }
            return query.ToList();
        }

        public IEnumerable<T> GroupingData(IEnumerable<string> groupingColumns,
          string[] includePaths = null)
        {
            IQueryable<T> data = _entities;

            IOrderedEnumerable<T> groupedData = null;

            if (includePaths != null)
            {
                for (var i = 0; i < includePaths.Count(); i++)
                {
                    data = data.Include(includePaths[i]);
                }
            }

            foreach (string grpCol in groupingColumns.Where(x => !String.IsNullOrEmpty(x)))
            {
                var col = typeof(T).GetProperty(grpCol, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

                if (col != null)
                {
                    groupedData = groupedData == null ? data.AsEnumerable().OrderBy(x => col.GetValue(x, null))
                                                   : groupedData.ThenBy(x => col.GetValue(x, null));
                }
            }
            return groupedData ?? data.AsEnumerable();
        }
    }
}
