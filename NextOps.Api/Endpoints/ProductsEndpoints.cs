using System;

namespace NextOps.Api.Endpoints;

public static class ProductsEndpoints
{
   public static void MapProductsEndpoint(this WebApplication app)
   {
      
      var group = app.MapGroup("/products");

      
      

   }

}
