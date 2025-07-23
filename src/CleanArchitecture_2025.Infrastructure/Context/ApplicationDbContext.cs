﻿using CleanArchitecture_2025.Domain.Abstraction;
using CleanArchitecture_2025.Domain.Employees;
using CleanArchitecture_2025.Domain.Users;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture_2025.Infrastructure.Context;

internal sealed class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Employee> Employees { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();

    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var enteries = ChangeTracker.Entries<Entity>();

        HttpContextAccessor httpContextAccessor = new();
        string userIdString = httpContextAccessor.HttpContext!.User.Claims.First(p=>p.Type == "user-id").Value;
        Guid userId = Guid.Parse(userIdString);

        foreach (var entry in enteries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreateAt)
                    .CurrentValue = DateTimeOffset.Now;
                entry.Property(p => p.CreateUserId)
                     .CurrentValue = userId;
            }
            if (entry.State == EntityState.Modified)
            {

                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeleteAt)
                        .CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.DeleteUserId)
                        .CurrentValue = userId;
                }
                else
                {
                    entry.Property(p => p.UpdateAt)
                        .IsModified = true;
                    entry.Property(p => p.UpdateUserId)
                        .CurrentValue = userId;
                }
            }
            if (entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("You can not delete this entity, you can only soft delete it.");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}
