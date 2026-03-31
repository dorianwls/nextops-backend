using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NextOps.Api.Database;
using NextOps.Api.Dtos.AppUser;
using NextOps.Api.Dtos.Users;
using NextOps.Api.Entities;
using NextOps.Api.Mapping;

namespace NextOps.Api.Endpoints.Users;

public static class UserTypedResults
{
   public static async Task<IResult> GetCurrentUser(
      ClaimsPrincipal claimsPrincipal,
      NextOpsContext db
   )
   {
      string userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
      string email = claimsPrincipal.FindFirstValue(ClaimTypes.Email)!;

      string[] permissions = claimsPrincipal.Claims
         .Where(claim => claim.Type == "permission")
         .Select(claim => claim.Value)
         .ToArray();

      List<Menu> flatMenu = await db.Menu
         .Where(menu => permissions.Contains(menu.RequiredClaim))
         .OrderBy(menu => menu.SectionOrder)
         .ThenBy(menu => menu.Order)
         .AsNoTracking()
         .ToListAsync();

      List<MenuItemDto> menu = BuildMenuTree(flatMenu);

      return TypedResults.Ok(new MeResponseDto(userId, email, menu));
   }

   private static List<Dtos.Users.MenuItemDto> BuildMenuTree(List<Menu> flatMenu)
        {
            Dictionary<int, MenuItemDto> map = flatMenu
                .Select(m => m.ToMenuItemDto())
                .ToDictionary(m => m.Id);

            List<MenuItemDto> menu = new();

            foreach (var menuItem in flatMenu)
            {
                MenuItemDto dto = map[menuItem.Id];

                if (menuItem.ParentMenuId is null)
                {
                    menu.Add(dto);
                }
                else if (map.TryGetValue(menuItem.ParentMenuId.Value, out var parentMenu))
                {
                    parentMenu.SubMenus.Add(dto);
                }
            }

            return menu;
        }




        public static async Task<IResult> GetAllAppUsers(UserManager<ApplicationUser> userManager)
   {
      var appUsers = await userManager.Users
         .AsNoTracking()
         .ToListAsync();

      return TypedResults.Ok(appUsers);
   }

   public static async Task<IResult> GetAppUser(string id, UserManager<ApplicationUser> userManager)
   {
      ApplicationUser? user = await userManager.Users
         .FirstOrDefaultAsync(u => u.Id == id);

      return user is null
         ? TypedResults.NotFound()
         : TypedResults.Ok(user.ToAppUserDto());

   }
   

   public static async Task<IResult> UpdateAppUser(
      string id,
      UpdateAppUserDto updateUser,
      UserManager<ApplicationUser> userManager
   )
   {
      ApplicationUser? user = await userManager.Users
         .FirstOrDefaultAsync(u => u.Id == id);

      if (user is null) return TypedResults.NotFound();

      updateUser.ToAppUserEntity(user);

      var result = await userManager.UpdateAsync(user);

      if (!result.Succeeded)
         return TypedResults.BadRequest(result.Errors);

      return TypedResults.NoContent();
   }

   public static async Task<IResult> DeleteAppUser(
      string id,
      UserManager<ApplicationUser> userManager
   )
   {
      ApplicationUser? user = await userManager.Users
         .FirstOrDefaultAsync(u => u.Id == id);

      if (user is null) return TypedResults.NotFound();

      var result = await userManager.DeleteAsync(user);

      if (!result.Succeeded)
         return TypedResults.BadRequest(result.Errors);

      return TypedResults.NoContent();
   }
}
