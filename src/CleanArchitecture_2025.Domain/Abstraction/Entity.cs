﻿using CleanArchitecture_2025.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture_2025.Domain.Abstraction
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.CreateVersion7();
        }
        public Guid Id { get; set; }


        #region Audit Log
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreateAt { get; set; }
        public Guid CreateUserId { get; set; } = default!;
        public string CreateUserName => GetCreateUserName();
        public DateTimeOffset? UpdateAt { get; set; }
        public Guid? UpdateUserId { get; set; }
        public string? UpdateUserName => GetUpdateUserName();
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeleteAt { get; set; }
        public Guid? DeleteUserId { get; set; }
        public string? DeleteUserName => GetDeleteUserName();

        private string GetCreateUserName()
        {
            HttpContextAccessor httpContextAccessor = new();
            var userManager = httpContextAccessor
                              .HttpContext
                              .RequestServices
                              .GetRequiredService<UserManager<AppUser>>();
            AppUser appUser = userManager.Users.First(p => p.Id == CreateUserId);

            return appUser.FirstName + " " + appUser.LastName + " (" + appUser.Email + ")";

        }
        private string? GetUpdateUserName()
        {
            if (UpdateUserId == null)
                return null;

            HttpContextAccessor httpContextAccessor = new();
            var userManager = httpContextAccessor
                              .HttpContext
                              .RequestServices
                              .GetRequiredService<UserManager<AppUser>>();
            AppUser appUser = userManager.Users.First(p => p.Id == UpdateUserId);

            return appUser.FirstName + " " + appUser.LastName + " (" + appUser.Email + ")";
        }
        private string? GetDeleteUserName()
        {

            if (DeleteUserId == null)
                return null;


            HttpContextAccessor httpContextAccessor = new();
            var userManager = httpContextAccessor
                              .HttpContext
                              .RequestServices
                              .GetRequiredService<UserManager<AppUser>>();
            AppUser appUser = userManager.Users.First(p => p.Id == DeleteUserId);

            return appUser.FirstName + " " + appUser.LastName + " (" + appUser.Email + ")";
        }
        #endregion
    }
}
