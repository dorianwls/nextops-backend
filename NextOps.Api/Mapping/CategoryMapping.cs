using System;
using NextOps.Api.Dtos.Categories;
using NextOps.Api.Entities;

namespace NextOps.Api.Mapping;

public static class CategoryMapping
{
   public static CategoryItemDto ToCategoryDto(this Category category)
   {
      return new CategoryItemDto
      {
         Id = product.Id,
         Name = product.Name,
         Model = product.Model,
         Brand = product.Brand,
         Description = product.Description,
         AverageCost = product.AverageCost,
         Stock = product.Stock,
         CategoryId = product.CategoryId,
         CategoryName = product.Category.Name,
         Status = product.Status
      };
      
   }
