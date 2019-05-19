using System;
using System.Linq;
using carbon.core.Features;

namespace carbon.persistence.interfaces
{
    public interface IReadWriteRepository : IReadOnlyRepository
    {
        void Create<T, TLd>(T entity) where T : Entity<TLd> where TLd : struct;
        
        void Update<T, TLd>(T entity) where T : Entity<TLd> where TLd : struct;
        
        void Delete<T, TLd>(T entity) where T : Entity<TLd> where TLd : struct;
        
        void Delete<T, TLd>(TLd id) where T : Entity<TLd> where TLd : struct;

        void Commit();
    }
}