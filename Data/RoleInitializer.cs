using fleet_tracking.Models;
using Microsoft.AspNetCore.Identity;

public static class RoleInitializer
{
    public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Define roles
        string[] roleNames = { "Admin", "SecurityPersonnel" };

        foreach (var roleName in roleNames)
        {
            // Check if the role already exists
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                // Create the role if it doesn't exist
                var role = new IdentityRole(roleName);
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Error creating role '{roleName}'.");
                }
            }
        }
    }
}
