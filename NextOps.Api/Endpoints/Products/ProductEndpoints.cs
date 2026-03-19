using System;

namespace NextOps.Api.Endpoints;

public static class ProductEndpoints
{
   public static RouteGroupBuilder MapProductsEndpoint(this WebApplication app)
   {
      
      var route = app.MapGroup("/products");

      route.MapGet("/", GetAllProducts).RequireAuthorization();
      route.MapGet("/{id}", GetProduct).RequiredAuthorization();
      route.MapPost("/", CreateProduct);
      route.MapPut("/", UpdateProduct);
      route.MapDelete("/{id}", DeleteProduct);

      return route;

   }

}
