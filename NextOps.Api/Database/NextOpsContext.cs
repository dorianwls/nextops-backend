using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NextOps.Api.Configurations;
using NextOps.Api.Entities;

namespace NextOps.Api.Database;

public class NextOpsContext(DbContextOptions<NextOpsContext> options) : IdentityDbContext<IdentityUser>(options)
{
   public DbSet<Menu> Menu { get; set; }
   

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<IdentityUser>().ToTable("user");
      modelBuilder.Entity<IdentityRole>().ToTable("role");
      modelBuilder.Entity<IdentityUserRole<string>>().ToTable("user_role");
      modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("user_claim");
      modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("role_claim");
      modelBuilder.Entity<IdentityUserToken<string>>().ToTable("user_token");
      modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("user_login");

      //Invoke Configurations
      new MenuConfiguration().Configure(modelBuilder.Entity<Menu>());

   }


 }
