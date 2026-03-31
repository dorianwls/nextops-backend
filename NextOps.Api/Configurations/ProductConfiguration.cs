
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextOps.Api.Entities;


namespace NextOps.Api.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
   public void Configure(EntityTypeBuilder<Product> builder)
   {
      builder.HasKey(p => p.Id);
      builder.Property(p => p.Id).ValueGeneratedOnAdd();
      builder.Property(p => p.Name).HasMaxLength(100);
      builder.Property(p => p.Model).HasMaxLength(100);
      builder.Property(p => p.Brand).HasMaxLength(100);
      builder.Property(p => p.Description).HasMaxLength(200);
      builder.HasOne(p => p.Category)
         .WithMany(c => c.Products)
         .HasForeignKey(p => p.CategoryId)
         .OnDelete(DeleteBehavior.Restrict);
      builder.Property(p => p.Status)
         .HasDefaultValue(true);

   }
}