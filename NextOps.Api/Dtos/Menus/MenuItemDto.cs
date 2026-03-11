using System;

namespace NextOps.Api.Dtos.Menus;

public class MenuItemDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Route { get; set; }
    public string? Icon { get; set; }
    public string? Section { get; set; }
    public List<MenuItemDto> SubMenus { get; } = [];
}
