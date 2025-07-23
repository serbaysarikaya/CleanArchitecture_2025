﻿using CleanArchitecture_2025.Domain.Abstraction;
using CleanArchitecture_2025.Domain.Employees;
using CleanArchitecture_2025.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture_2025.Application.Employees
{
    public sealed record EmployeeGetAllQuery() : IRequest<IQueryable<EmployeeGetAllQueryResponse>>;

    public sealed class EmployeeGetAllQueryResponse : EntityDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateOnly BirthOfDate { get; set; }
        public decimal Salary { get; set; }
        public string TCNo { get; set; } = default!;
    }

    internal sealed class EmployeeGetAllIQueryHandler(
        IEmployeeRepository employeeRepository,
        UserManager<AppUser> userManager) : IRequestHandler<EmployeeGetAllQuery,
        IQueryable<EmployeeGetAllQueryResponse>>
    {
        public Task<IQueryable<EmployeeGetAllQueryResponse>> Handle(EmployeeGetAllQuery request, CancellationToken cancellationToken)
        {
            var response = (from employee in employeeRepository.GetAll()
                            join create_user in userManager.Users.AsQueryable() on employee.CreateUserId equals create_user.Id
                            join update_user in userManager.Users.AsQueryable() on employee.CreateUserId equals update_user.Id into update_user
                            from update_users in update_user.DefaultIfEmpty()
                            select new EmployeeGetAllQueryResponse
                            {
                                FirstName = employee.FirstName,
                                LastName = employee.LastName,
                                BirthOfDate = employee.BirthOfDate,
                                Salary = employee.Salary,
                                CreateAt = employee.CreateAt,
                                UpdateAt = employee.UpdateAt,
                                DeleteAt = employee.DeleteAt,
                                Id = employee.Id,
                                IsDeleted = employee.IsDeleted,
                                TCNo = employee.PersonelInformation.TCNo,
                                CreateUserId = employee.CreateUserId,
                                CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ") ",
                                UpdateUserId = employee.UpdateUserId,
                                UpdateUserName = employee.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ") "
                            }).AsQueryable();
            return Task.FromResult(response);
        }
    }
}
