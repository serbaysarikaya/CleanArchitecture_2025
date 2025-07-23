﻿using CleanArchitecture_2025.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture_2025.WebAPI;

public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using (var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.Users.Any(p => p.UserName == "admin"))
            {
                AppUser user = new()
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "Serbay",
                    LastName = "Sarıkaya",
                    EmailConfirmed = true,
                    CreateAt= DateTimeOffset.Now,
                  

                };
                user.CreateUserId = user.Id; // Assuming you have a way to set the ID of the creator

                userManager.CreateAsync(user, "1").Wait();
            }
        }
    }
}
