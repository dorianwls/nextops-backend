using System;
using Microsoft.EntityFrameworkCore;
using NextOps.Api.Database;
using NextOps.Api.Dtos.Categories;
using NextOps.Api.Entities;
using NextOps.Api.Mapping;

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

   public static async Task<IResult> CreateCategory(Category category, NextOpsContext dbContext)
   {
      dbContext.Category.Add(category);
      await dbContext.SaveChangesAsync();

      return TypedResults.Created($"/category/{category.Id}", category);
   }

   public static async Task<IResult> UpdateCategory(
      int id,
      UpdateCategoryDto updateCategory,
      NextOpsContext dbContext
   )
   {
      Category? category = await dbContext.Category.FindAsync(id);
      if (category is null) return TypedResults.NotFound();

      dbContext.Entry(category).CurrentValues.SetValues(updateCategory);
      await dbContext.SaveChangesAsync();

      return TypedResults.NoContent();
   }

   public static async Task<IResult> DeleteCategory(
      int id,
      NextOpsContext dbContext
   )
   {
      Category? category = await dbContext.Category.FindAsync(id);
      if (category is null) return TypedResults.NotFound();

      dbContext.Category.Remove(category);
      await dbContext.SaveChangesAsync();

      return TypedResults.NoContent();
   }

}
