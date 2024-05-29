using EvAldarado.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EvAldarado.DAL
{
	public class AppDBContext : IdentityDbContext<Users>
	{
		public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
		{

		}

        public DbSet<Users> Users { get; set; }
        public DbSet<UserProducts> UserProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship between Users and UserProducts
            modelBuilder.Entity<UserProducts>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProducts)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<Images> Images { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<Category> Categories { get; set; } 
		public DbSet<AppRole> AppRoles { get; set; }	

    }
}
