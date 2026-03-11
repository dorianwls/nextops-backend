using System;
using NextOps.Api.Dtos.Menus;
using NextOps.Api.Entities;

namespace NextOps.Api.Mapping;

public static class MenuMapping
{
   public static MenuItemDto ToMenuItemDto(this Menu menu)
   {
      return new MenuItemDto
      {
         Id = menu.Id,
         Name =  menu.Name,
         Route = menu.Route,
         Icon = menu.Icon,
         Section = menu.Section
      };
   }

}
