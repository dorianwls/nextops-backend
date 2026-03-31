using System;

namespace NextOps.Api.Endpoints.Users;
using static NextOps.Api.Endpoints.Users.UserTypedResults;

public static class UserEndpoints
{
   public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
   {
      var route = app.MapGroup("/users");
      route.RequireAuthorization();
      route.MapGet("/me", GetCurrentUser);

      route.MapGet("/", GetAllAppUsers);
      route.MapGet("/{id}", GetAppUser);
      route.MapPut("/{id}", UpdateAppUser);
      route.MapDelete("/{id}", DeleteAppUser);

      return route;
   }
}
