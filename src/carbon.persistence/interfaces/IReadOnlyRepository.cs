using System;
using System.Linq;
using carbon.core.Features;
using carbon.core.Interfaces;

namespace carbon.persistence.interfaces
{
    public interface IReadOnlyRepository : IDisposable
    {
        IQueryable Table<T, TLd>() where T : Entity<TLd> where TLd : struct;
        
        T GetById<T, TLd>() where T : Entity<TLd> where TLd : struct;
    }
}