using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NextOps.Api.Configurations;
using NextOps.Api.Entities;

namespace NextOps.Api.Database;

public class NextOpsContext(DbContextOptions<NextOpsContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, int>(options)
{
   public DbSet<Menu> Menu { get; set; }
   public DbSet<Product> Product {get; set;}
   public DbSet<Category> Category {get; set;}
   
   

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      //Identity
      modelBuilder.Entity<ApplicationUser>().ToTable("user");
      modelBuilder.Entity<ApplicationRole>().ToTable("role");

      modelBuilder.Entity<IdentityUserRole<int>>().ToTable("user_role");
      modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("user_claim");
      modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("role_claim");
      modelBuilder.Entity<IdentityUserToken<int>>().ToTable("user_token");
      modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("user_login");


      //Invoke Configurations
      new MenuConfiguration().Configure(modelBuilder.Entity<Menu>());
      new ProductConfiguration().Configure(modelBuilder.Entity<Product>());
      new CategoryConfiguration().Configure(modelBuilder.Entity<Category>());
      new AppUserConfiguration().Configure(modelBuilder.Entity<ApplicationUser>());
   }


}
