using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using SalesManagerSolution.Domain.Entities;
using System.Linq.Expressions;
using System.Reflection;

namespace SalesManagerSolution.Infrastructure.EntityFramework
{
	public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles  { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductInCategory> ProductInCategories { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Cart>()
				   .HasOne(x => x.AppUser)
				   .WithMany(i => i.Carts)
				   .HasForeignKey(x=>x.UserId);

			builder.Entity<ProductInCategory>()
				   .HasOne(x => x.Product)
				   .WithMany(i => i.ProductInCategories)
				   .HasForeignKey(x => x.ProductId);

			builder.Entity<ProductInCategory>()
				   .HasOne(x => x.Category)
				   .WithMany(i => i.ProductInCategories);

			builder.Entity<OrderDetail>()
				   .HasOne(x => x.Order)
				   .WithMany(i => i.OrderDetails)
				   .HasForeignKey(x => x.OrderId);

			builder.Entity<OrderDetail>()
				   .HasOne(x => x.Product)
				   .WithMany(i => i.OrderDetails)
				   .HasForeignKey(x => x.ProductId);

			builder.Entity<Order>()
				   .HasOne(x => x.AppUser)
				   .WithMany(i => i.Orders)
				   .HasForeignKey(x => x.UserId);

			builder.Entity<Cart>()
				   .HasOne(x => x.Product)
				   .WithMany(i => i.Carts)
				   .HasForeignKey(x => x.ProductId);
			builder.Entity<ProductImage>()
				   .HasOne(x => x.Product)
				   .WithMany(i => i.ProductImages)
				   .HasForeignKey(x => x.ProductId);

			builder.Entity<Transaction>()
				   .HasOne(x => x.AppUser)
				   .WithMany(i => i.Transactions)
				   .HasForeignKey(x => x.UserId);

			AddSoftDeleteFilters(builder);

			SeedData(builder);
		}
		private static void AddSoftDeleteFilters(ModelBuilder builder)
		{
			foreach (var entityType in builder.Model.GetEntityTypes())
			{
				//other automated configurations left out
				if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
				{
					entityType.AddSoftDeleteQueryFilter();
				}
			}
		}


		/// <summary>
		/// Seed data for AppUser
		/// </summary>
		/// <returns></returns>

		private static void SeedData(ModelBuilder builder)
		{
			#region seed data for identity role
			// role admin
			builder.Entity<AppRole>().HasData(new AppRole { Id = 1, Name = "Administrator", NormalizedName = "ADMINISTRATOR".ToUpper(), Description = "Admin role" });

			//role applicant
			builder.Entity<AppRole>().HasData(new AppRole { Id = 2, Name = "Applicant", NormalizedName = "APPLICANT".ToUpper(), Description = "Admin role" });
			#endregion

			#region seed data for identity user
			// create new user admin
			var appUser = new AppUser
			{
				Id = 1,
				UserName = "admin@gmail.com",
				Email = "admin@gmail.com",
				NormalizedUserName = "ADMIN",
				FirstName = "Admin",
				LastName = "Kien",
				NormalizedEmail = "ADMIN@GMAIL.COM",
				SecurityStamp = Guid.NewGuid().ToString()
			};

			// create password for user admin
			var hasher = new PasswordHasher<AppUser>();
			appUser.PasswordHash = hasher.HashPassword(appUser, "admin");

			// seed data
			builder.Entity<AppUser>().HasData(appUser);


			// create new user applicant
			var userApplicant = new AppUser
			{
				Id = 2,
				UserName = "applicant@gmail.com",
				Email = "applicant@gmail.com",
				NormalizedUserName = "APLLICANT",
				FirstName = "applicant",
				LastName = "Truong",
				NormalizedEmail = "APLLICANT@GMAIL.COM",
				SecurityStamp = Guid.NewGuid().ToString()
			};

			// create password for user applicant
			var hasherApplicant = new PasswordHasher<AppUser>();
			userApplicant.PasswordHash = hasherApplicant.HashPassword(userApplicant, "applicant");

			builder.Entity<AppUser>().HasData(userApplicant);
			#endregion

			#region seed data for identity userrole
			builder.Entity<IdentityUserRole<int>>().HasData(
				new IdentityUserRole<int>
				{
					RoleId = 1,
					UserId = 1
				});

			builder.Entity<IdentityUserRole<int>>().HasData(
				new IdentityUserRole<int>
				{
					RoleId = 2,
					UserId = 2
				});
            #endregion

            #region Seed data for category
            builder.Entity<Category>().HasData(
                new Category
                {
					Id = 1,
                    Name = "Women",
					Description = "Women",
					IsShowOnHome = true,
					SortOrder = 1
                });

            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 2,
                    Name = "Men",
                    Description = "Men",
                    IsShowOnHome = true,
                    SortOrder = 2
                });
            builder.Entity<Category>().HasData(
                new Category
                {
					Id = 3,
                    Name = "Shoes",
                    Description = "Shoes",
                    IsShowOnHome = true,
                    SortOrder = 3
                });

            builder.Entity<Category>().HasData(
                new Category
                {
					Id = 4,
                    Name = "Watches",
                    Description = "Watches",
                    IsShowOnHome = true,
                    SortOrder = 4
                });
            #endregion
        }

        public override int SaveChanges()
		{
			UpdateEntityState();
			return base.SaveChanges();
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			UpdateEntityState();
			return base.SaveChangesAsync(cancellationToken);
		}

		private void UpdateEntityState()
		{
			var now = DateTime.UtcNow;

			foreach (var changedEntity in ChangeTracker.Entries())
			{
				if (changedEntity.Entity is BaseEntity entity)
				{
					switch (changedEntity.State)
					{
						case EntityState.Added:
							entity.CreatedAt = now;
							entity.UpdatedAt = now;
							break;
						case EntityState.Modified:
							Entry(entity).Property(x => x.CreatedAt).IsModified = false;
							entity.UpdatedAt = now;
							break;
						case EntityState.Deleted:
							entity.IsDeleted = true;
							entity.DeletedAt = now;
							changedEntity.State = EntityState.Modified;
							break;
					}
				}
			}
		}
	}
	public static class SoftDeleteQueryExtension
	{
		public static void AddSoftDeleteQueryFilter(
			this IMutableEntityType entityData)
		{
			var methodToCall = typeof(SoftDeleteQueryExtension)
				.GetMethod(nameof(GetSoftDeleteFilter),
					BindingFlags.NonPublic | BindingFlags.Static)
				?.MakeGenericMethod(entityData.ClrType);
			var filter = methodToCall?.Invoke(null, new object[] { });
			if (filter != null)
			{
				entityData.SetQueryFilter((LambdaExpression)filter);
			}
		}

		private static LambdaExpression GetSoftDeleteFilter<TEntity>()
			where TEntity : BaseEntity
		{
			Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
			return filter;
		}
	}
}
