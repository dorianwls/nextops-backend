using System;
using NextOps.Api.Entities;
using NextOps.Api.Dtos.Products;

namespace NextOps.Api.Mapping;

public static class ProductMapping
{
   public static ProductItemDto ToProductDto(this Product product)
   {
      return new ProductItemDto
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
}
