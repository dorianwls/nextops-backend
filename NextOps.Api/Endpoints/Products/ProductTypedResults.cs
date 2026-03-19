using System;
using Microsoft.EntityFrameworkCore;
using NextOps.Api.Database;
using NextOps.Api.Dtos.Products;
using NextOps.Api.Entities;
using NextOps.Api.Mapping;

namespace NextOps.Api.Endpoints.Products;

public static class ProductTypedResults
{

   public static async Task<IResult> GetAllProducts(NextOpsContext dbContext)
   {
      var products = await dbContext.Product.AsNoTracking().ToListAsync();

      return TypedResults.Ok(products);
   }

   public static async Task<IResult> GetProduct(int id, NextOpsContext dbContext)
   {
      Product? product = await dbContext.Product
         .Include(p => p.Category)
         .FirstOrDefaultAsync(p => p.Id == id);

      return product is null? TypedResults.NotFound() : TypedResults.Ok(product.ToProductDto());
   }

   public static async Task<IResult> CreateProduct(Product product, NextOpsContext dbContext)
   {
      dbContext.Product.Add(product);
      await dbContext.SaveChangesAsync();

      return TypedResults.Created($"/products/{product.Id}", product);
   }

   public static async Task<IResult> UpdateProduct(
      int id,
      UpdateProductDto updateProduct,
      NextOpsContext dbContext
   )
   {
      Product? product = await dbContext.Product.FindAsync(id);
      if(product is null) return TypedResults.NotFound();

      dbContext.Entry(product).CurrentValues.SetValues(updateProduct);
      await dbContext.SaveChangesAsync();

      return TypedResults.NoContent();
   }

   public static async Task<IResult> DeleteProduct(int id, NextOpsContext dbContext)
   {
      Product? product = await dbContext.Product.FindAsync(id);
      if(product is null) return TypedResults.NotFound();

      dbContext.Product.Remove(product);
      await dbContext.SaveChangesAsync();

      return TypedResults.NoContent();
   }

}
