using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextOps.Api.Entities;

namespace NextOps.Api.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
   public void Configure(EntityTypeBuilder<Menu> builder)
   {
      builder.HasKey(m => m.Id);
      builder.Property(m => m.Id).ValueGeneratedOnAdd();

      builder.Property(m => m.Name).IsRequired().HasMaxLength(200);
      builder.Property(m => m.Route).HasMaxLength(200);
      builder.Property(m => m.Icon).HasMaxLength(100);
      builder.Property(m => m.Section).HasMaxLength(200);
      builder.Property(m => m.RequiredClaim);
      builder.Property(m => m.Order).HasDefaultValue(0);

   }
}
