using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextOps.Api.Entities;

namespace NextOps.Api.Configurations;

public class CategoryConfiguration
{
   public void Configure(EntityTypeBuilder<Category> builder)
   {
         builder.HasKey(c => c.Id);

         builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

         builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

         builder.Property(c => c.Description)
            .HasMaxLength(200);

         builder.Property(c => c.Status)
            .HasDefaultValue(true);

   }
}
