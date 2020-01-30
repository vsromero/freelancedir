using FreelanceDir.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FreelanceDir.Data
{
    public class RDSContext : IdentityDbContext<User>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RDSContext(DbContextOptions<RDSContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var entities = ChangeTracker.Entries().Where(e => e.Entity is BaseModel && (e.State == EntityState.Added || e.State == EntityState.Modified));

                foreach (var entity in entities)
                {
                    if (entity.State == EntityState.Added)
                    {
                        entity.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                    }
                    entity.Property("LastModifiedDate").CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);            
            builder.Entity<Order>()
                .HasOne(o => o.Package)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<FreelanceDir.Models.Gig> Gigs { get; set; }
        public DbSet<FreelanceDir.Models.Category> Categories { get; set; }
        public DbSet<FreelanceDir.Models.Package> Packages { get; set; }
        public DbSet<FreelanceDir.Models.Review> Reviews { get; set; }
        public DbSet<FreelanceDir.Models.Order> Orders { get; set; }
        public DbSet<FreelanceDir.Models.Message> Messages { get; set; }
    }
}
