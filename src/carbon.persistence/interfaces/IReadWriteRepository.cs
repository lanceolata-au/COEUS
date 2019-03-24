using System;
using System.Linq;
using carbon.core.Features;

namespace carbon.persistence.interfaces
{
    public interface IReadWriteRepository : IDisposable
    {
        IQueryable Table<T, TLd>() where T : Entity<TLd> where TLd : struct;
        
        T GetById<T, TLd>() where T : Entity<TLd> where TLd : struct;
    }
}