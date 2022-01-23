using Entity;
using Entity.AuthEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ReadLaterDataContext : IdentityDbContext<User>

    {
        public ReadLaterDataContext(DbContextOptions<ReadLaterDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Bookmark> Bookmark { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

        public override int SaveChanges()
        {
    
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Bookmark && (e.State == EntityState.Added));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((Bookmark)entityEntry.Entity).CreateDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}
