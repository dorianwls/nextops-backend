using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NextOps.Api.Entities;

namespace NextOps.Api.Database;

public class IdentityDataSeeder
{
public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        // ==========================================
        // STEP 1: Define and Create the Roles
        // ==========================================
        string adminRoleName = "Admin";
        string basicRoleName = "BasicViewer";

        // Create Admin Role
        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = adminRoleName });
        }

        // Create Basic Viewer Role (for self-registered users)
        if (!await roleManager.RoleExistsAsync(basicRoleName))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = basicRoleName });
        }


        // ==========================================
        // STEP 2: Assign Claims (Permissions) to Roles
        // ==========================================
        var adminRole = await roleManager.FindByNameAsync(adminRoleName);
        var existingAdminClaims = await roleManager.GetClaimsAsync(adminRole!);

         if (adminRole != null) 
         {

            foreach (var menuItem in MenuSeed.Menu)
            {
               // 2. Validamos que el RequiredClaim no venga nulo o vacío desde tu menú
               if (!string.IsNullOrEmpty(menuItem.RequiredClaim)) 
               {
                     if (!existingAdminClaims.Any(c => c.Value == menuItem.RequiredClaim))
                     {
                        // Ya no necesitamos el '!' porque el 'if' de arriba le garantiza al compilador que no es nulo
                        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", menuItem.RequiredClaim));
                     }
               }
            }
         }

        // Optional: Give the Basic role just one safe permission
        var basicRole = await roleManager.FindByNameAsync(basicRoleName);
        var existingBasicClaims = await roleManager.GetClaimsAsync(basicRole!);
        
        if (!existingBasicClaims.Any(c => c.Value == "dashboard:read"))
        {
            await roleManager.AddClaimAsync(basicRole!, new Claim("Permission", "dashboard:read"));
        }


        // ==========================================
        // STEP 3: Create the Initial Admin User
        // ==========================================
        string adminEmail = "admin@nextops.com";
        var systemAdminUser = await userManager.FindByEmailAsync(adminEmail);

        if (systemAdminUser == null)
        {
            systemAdminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                // Add any extra fields you created in your custom ApplicationUser class here
                // e.g., FirstName = "System", LastName = "Admin"
            };

            // This hashes the password and creates the user in the AspNetUsers table
            await userManager.CreateAsync(systemAdminUser, "SuperSecurePassword123!");
        }


        // ==========================================
        // STEP 4: Assign the User to the Role
        // ==========================================
        
        // Ensure the newly created (or existing) user actually has the Admin role
        if (!await userManager.IsInRoleAsync(systemAdminUser, adminRoleName))
        {
            await userManager.AddToRoleAsync(systemAdminUser, adminRoleName);
        }
    }
}
