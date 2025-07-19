using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture_2025.Domain.Users
{
    public sealed class AppUser:IdentityUser<Guid>
    {
        public AppUser()
        {
            Id = Guid.CreateVersion7();
        }
        public string FirstName { get; set; }=default!;
        public string LastName { get; set; } = default!;
        public string FulName => $"{FirstName} {LastName}"; //computed property


    }
}
