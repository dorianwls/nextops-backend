using System;
using Microsoft.EntityFrameworkCore;
using NextOps.Api.Database;
using NextOps.Api.Entities;

namespace NextOps.Api.Endpoints.Products;

public class ProductTypedResults
{

   public static async Task<IResult> GetAllProducts(NextOpsContext dbContext)
   {
      var products = await dbContext.Product.AsNoTracking().ToListAsync();

      return TypedResults.Ok(products);
   }

   public static async Task<IResult> GetClient(int id, NextOpsContext dbContext)
   {
      Product? product = await dbContext.Product
         .FirstOrDefaultAsync(p => p.Id == id);

      return product is null? TypedResults.NotFound() : TypedResults.Ok(product.ToProductDto());
   }

}
