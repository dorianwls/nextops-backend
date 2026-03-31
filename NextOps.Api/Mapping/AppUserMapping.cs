using System;
using NextOps.Api.Dtos.AppUser;
using NextOps.Api.Entities;

namespace NextOps.Api.Mapping;

public static class AppUserMapping
{
   public static AppUserItemDto ToAppUserDto(this ApplicationUser user)
   {
      return new AppUserItemDto
      {
         Id = user.Id,
         Email = user.Email,
         UserName = user.UserName,
         PhoneNumber = user.PhoneNumber,
         FirstName = user.FirstName,
         MiddleName = user.MiddleName,
         LastName = user.LastName,
         SecondLastname = user.SecondLastname,
         Title = user.Title
      };
   }
}
