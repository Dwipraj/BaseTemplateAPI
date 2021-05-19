using BaseTemplateAPI.Entity.Table;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaseTemplateAPI.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext([NotNull] DbContextOptions options) : base(options)
        {
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<AppUser>(entity =>
			{
				entity.HasIndex(u => u.NormalizedUserName).IsUnique();
				entity.HasIndex(u => u.NormalizedEmail).IsUnique();
				entity.HasIndex(u => u.PhoneNumber).IsUnique();

				entity.Property(u => u.Name).HasMaxLength(100).IsRequired();
				entity.Property(u => u.Email).IsRequired();
				entity.Property(u => u.PhoneNumber).IsRequired();
			});
		}

		public override int SaveChanges()
		{
			AddTimestamps();
			return base.SaveChanges();
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			AddTimestamps();
			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			AddTimestamps();
			return await base.SaveChangesAsync(cancellationToken);
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			AddTimestamps();
			return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		private void AddTimestamps()
		{
			var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

			foreach (var entity in entities)
			{
				var now = DateTime.Now;

				if (entity.State == EntityState.Added)
				{
					((BaseEntity)entity.Entity).CreatedAt = now;
				}
				else
				{
					((BaseEntity)entity.Entity).UpdatedAt = now;
				}
			}
		}
	}
}
