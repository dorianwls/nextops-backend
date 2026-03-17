using System;

namespace NextOps.Api.Endpoints.Users;

public static class UserEndpoints
{
   public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
   {
      var route = app.MapGroup("/users");
      route.RequireAuthorization();
      route.MapGet("/me", GetCurrentUser);
      return route;

   }
}
