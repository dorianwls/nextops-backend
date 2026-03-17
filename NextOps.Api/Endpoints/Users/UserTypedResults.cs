using System;
using System.Security.Claims;
using NextOps.Api.Database;

namespace NextOps.Api.Endpoints.Users;

public static class UserTypedResults
{
   public static async Task<IResult> GetCurrentUser(
      ClaimsPrincipal claimsPrincipal,
      NextOpsContext db
   )
   {
      
   }
}
