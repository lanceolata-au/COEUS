using System.Configuration;
using carbon.core.domain.model;
using carbon.core.domain.model.account;
using carbon.core.domain.model.registration;
using carbon.core.domain.model.registration.medical;
using carbon.core.domain.model.scoutEvent;
using carbon.persistence.interfaces;
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
            
            BuildApplicationModel(modelBuilder);
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Test> Test { get; set; }
        
        public DbSet<CoreUser> CoreUsers { get; set; }
        
        public DbSet<ScoutEvent> ScoutEvents { get; set; }
        
        public DbSet<Application> Applications { get; set; }

        private static void BuildApplicationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>()
                .HasOne(a => a.ApplicationMedical);

            modelBuilder.Entity<ApplicationMedical>()
                .HasMany(a => a.Allergies);
            
            modelBuilder.Entity<ApplicationMedical>()
                .HasMany(a => a.Conditions);
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        
        
        
    }
}