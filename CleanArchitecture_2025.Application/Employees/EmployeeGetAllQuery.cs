using CleanArchitecture_2025.Domain.Abstraction;
using CleanArchitecture_2025.Domain.Employees;
using MediatR;

namespace CleanArchitecture_2025.Application.Employees
{
    public sealed record EmployeeGetAllQuery() : IRequest<IQueryable<EmployeeGetAllQueryResponse>>;

    public sealed class EmployeeGetAllQueryResponse : EntityDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string TCNo { get; set; } = default!;
        public DateOnly BirthOfDate { get; set; }
        public decimal Salary { get; set; }
        public Addrees? Address { get; set; }
    }

    internal sealed class EmployeeGetAllIQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<EmployeeGetAllQuery,
        IQueryable<EmployeeGetAllQueryResponse>>
    {
        public Task<IQueryable<EmployeeGetAllQueryResponse>> Handle(EmployeeGetAllQuery request, CancellationToken cancellationToken)
        {
            var response = employeeRepository.GetAll().Select(x => new EmployeeGetAllQueryResponse
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                TCNo = x.PersonalInformation.TCNo,
                BirthOfDate = x.BirthOfDate,
                Salary = x.Salary,
                Address = x.Address,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                IsDeleted = x.IsDeleted,
                DeleteAt = x.DeleteAt,
                Id = x.Id
            })
            .AsQueryable();
            return Task.FromResult(response);
        }
    }
}
