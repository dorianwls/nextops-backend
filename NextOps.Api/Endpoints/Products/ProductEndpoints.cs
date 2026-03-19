using System;
using static NextOps.Api.Endpoints.Products.ProductTypedResults;
namespace NextOps.Api.Endpoints;

public static class ProductEndpoints
{
   public static RouteGroupBuilder MapProductsEndpoint(this WebApplication app)
   {
      
      var route = app.MapGroup("/products");

      route.MapGet("/", GetAllProducts).RequireAuthorization();
      route.MapGet("/{id}", GetProduct).RequireAuthorization();
      route.MapPost("/", CreateProduct);
      route.MapPut("/", UpdateProduct);
      route.MapDelete("/{id}", DeleteProduct);

      return route;

   }

}
