using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carbon.core.Features;
using carbon.core.Interfaces;

namespace carbon.persistence.interfaces
{
    public interface IReadOnlyRepository : IDisposable
    {
        IEnumerable<T> Table<T, TLd>(
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where T : Entity<TLd> where TLd : struct;
        
        T GetById<T, TLd>(TLd id) where T : Entity<TLd> where TLd : struct;
        
        Task<T> GetByIdAsync<T, TLd>(TLd id) where T : Entity<TLd> where TLd : struct;
    }
}