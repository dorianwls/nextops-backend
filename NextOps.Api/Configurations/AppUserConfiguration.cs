using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextOps.Api.Entities;

namespace NextOps.Api.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
   public void Configure(EntityTypeBuilder<ApplicationUser> builder)
   {
      builder.Property(u => u.FirstName).HasMaxLength(100);
      builder.Property(u => u.MiddleName).HasMaxLength(100);
      builder.Property(u => u.LastName).HasMaxLength(100);
      builder.Property(u => u.SecondLastname).HasMaxLength(100);
      builder.Property(u => u.Title).HasMaxLength(100);
   }
}
