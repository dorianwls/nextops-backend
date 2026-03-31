using System;
using Microsoft.EntityFrameworkCore;
using NextOps.Api.Database;
using NextOps.Api.Entities;

namespace NextOps.Api.Endpoints.Categories;

public static class CategoryTypedResults
{
   public static async Task<IResult> GetAllCategories(NextOpsContext dbContext)
   {
      var categories = await dbContext.Category.AsNoTracking().ToListAsync();

      return TypedResults.Ok(categories);
   }

   public static async Task<IResult> GetCategory(int id, NextOpsContext dbContext)
   {
      Category? category = await dbContext.Category
         .FirstOrDefaultAsync(c => c.Id == id);

      return category is null? TypedResults.NotFound() : TypedResults.Ok(category.ToCategoryDto());
   }
}
