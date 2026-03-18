using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NextOps.Api.Database;
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
}
