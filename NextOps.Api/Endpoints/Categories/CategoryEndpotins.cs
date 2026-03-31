using static NextOps.Api.Endpoints.Categories.CategoryTypedResults;

namespace NextOps.Api.Endpoints.Categories;

public static class CategoryEndpotins
{
   public static RouteGroupBuilder MapCategoriesEndpoint(this WebApplication app)
   {
      var route = app.MapGroup("/categories");

      route.MapGet("/", GetAllCategories);
      route.MapGet("/{id}", GetCategory);
      route.MapPost("/", CreateCategory);
      route.MapPut("/", UpdateCategory);
      route.MapDelete("/{id}", DeleteCategory);

      return route;
   }
}
