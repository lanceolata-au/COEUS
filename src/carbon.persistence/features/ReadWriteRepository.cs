using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using carbon.core.Features;
using carbon.persistence.interfaces;
using Microsoft.EntityFrameworkCore;

namespace carbon.persistence.features
{
    public class ReadWriteRepository : IReadWriteRepository
    {
        
        private readonly DbContext _context;    

        public ReadWriteRepository(DbContext context)
        {
            _context = context;
        }
        
        public void Dispose()
        {
            try
            {
                var result = _context.SaveChangesAsync().Result;
                Debug.WriteLine("Disposing " + this.GetHashCode() + " WITH Res:" + result);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        protected virtual IQueryable<T> GetQueryable<T, TLd>(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where T : Entity<TLd> where TLd : struct
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public virtual IEnumerable<T> Table<T, TLd>(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null) where T : Entity<TLd> where TLd : struct
        {
            return GetQueryable<T, TLd>(null, orderBy, includeProperties, skip, take).ToList();
        }

        public virtual T GetById<T, TLd>(TLd id) where T : Entity<TLd> where TLd : struct
        {
            return _context.Set<T>().Find(id);
        }

        public virtual Task<T> GetByIdAsync<T, TLd>(TLd id) where T : Entity<TLd> where TLd : struct
        {
            return _context.Set<T>().FindAsync(id).AsTask();
        }

        public virtual void Create<T, TLd>(T entity) where T : Entity<TLd> where TLd : struct
        {
            _context.Set<T>().Add(entity);
        }

        public virtual void Update<T, TLd>(T entity) where T : Entity<TLd> where TLd : struct
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete<T, TLd>(T entity) where T : Entity<TLd> where TLd : struct
        {
            var dbSet = _context.Set<T>();
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            dbSet.Remove(entity);

        }

        public virtual void Delete<T, TLd>(TLd id) where T : Entity<TLd> where TLd : struct
        {
            var entity = _context.Set<T>().Find(id);
            
            Delete<T, TLd>(entity);
            
        }

        
        /// <summary>
        /// </summary>
        /// 
        /// <remarks>
        /// WARNING!!
        /// This should not be used in most cases. Instead disposable will take care of saving to the DB!!
        /// WARNING!!
        /// </remarks>
        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}