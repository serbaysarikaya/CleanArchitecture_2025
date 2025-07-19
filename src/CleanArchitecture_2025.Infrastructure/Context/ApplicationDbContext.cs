﻿using CleanArchitecture_2025.Domain.Abstraction;
using CleanArchitecture_2025.Domain.Employees;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture_2025.Infrastructure.Context;

internal sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var enteries = ChangeTracker.Entries<Entity>();

        foreach (var entry in enteries)
        {
            if(entry.State == EntityState.Added)
            {
                entry.Property(p=>p.CreateAt)
                    .CurrentValue =DateTimeOffset.Now;
            }
            if (entry.State == EntityState.Modified)
            {

                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeleteAt)
                        .CurrentValue = DateTimeOffset.Now;
                }
                else
                {
                    entry.Property(p => p.UpdateAt)
                        .IsModified = true;
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
