using carbon.core.domain.model;
using Microsoft.EntityFrameworkCore;

namespace carbon.persistence.features
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions options) : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        
        
        
        public DbSet<Test> Test { get; set; }
        
    }
}