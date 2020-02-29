using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using carbon.core.Features;
using carbon.core.Interfaces;
using carbon.persistence.interfaces;
using Microsoft.EntityFrameworkCore;

namespace carbon.persistence.features
{
    public class ReadOnlyRepository : IReadOnlyRepository
    {
        private readonly DbContext _context;

        public ReadOnlyRepository(DbContext context)
        {
            _context = context;
        }
        
        public void Dispose()
        {
            //We don't need to do anything on a read repo dispose as it cannot commit data
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
    }
}