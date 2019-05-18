using System.Linq;
using System.Threading.Tasks;
using carbon.core.Features;
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

        public IQueryable Table<T, TLd>() where T : Entity<TLd> where TLd : struct
        {
            throw new System.NotImplementedException();
        }

        public virtual T GetById<T, TLd>(TLd id) where T : Entity<TLd> where TLd : struct
        {
            return _context.Set<T>().Find(id);
        }

        public virtual Task<T> GetByIdAsync<T, TLd>(TLd id) where T : Entity<TLd> where TLd : struct
        {
            return _context.Set<T>().FindAsync(id);
        }
    }
}