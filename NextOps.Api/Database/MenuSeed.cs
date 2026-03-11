using System;
using NextOps.Api.Entities;

namespace NextOps.Api.Database;

public class MenuSeed
{  
   public static readonly Menu[] Menu =
    [
        new Menu
        {
            Name = "Dashboard",
            Route = "/dashboard",
            Icon = "IconLayoutDashboard",
            Order = 1,
            Section = "Operación",
            SectionOrder = 1,
            RequiredClaim = "dashboard:read",
        },
        new Menu
        {
            Name = "Reportes",
            Route = "/reports",
            Icon = "IconReportAnalytics",
            Order = 5,
            Section = "Operación",
            SectionOrder = 1,
            RequiredClaim = "reports:read",
        },
        new Menu
        {
            Name = "Usuarios",
            Route = "/users",
            Icon = "IconUser",
            Order = 1,
            Section = "Seguridad",
            SectionOrder = 2,
            RequiredClaim = "users:read",
        },
        new Menu
        {
            Name = "Roles",
            Route = "/roles",
            Icon = "IconShield",
            Order = 2,
            Section = "Seguridad",
            SectionOrder = 2,
            RequiredClaim = "roles:read",
        },
        new Menu
        {
            Name = "Configuración",
            Route = "/settings",
            Icon = "settings",
            Order = 1,
            Section = "Configuración",
            SectionOrder = 3,
            RequiredClaim = "settings:read",
        },
    ];
}
