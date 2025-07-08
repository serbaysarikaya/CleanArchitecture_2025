using CleanArchitecture_2025.Domain.Employees;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Employees;

public sealed record EmployeeCreateCommand
(
    string FirstName,
    string LastName,
    DateOnly BirthOfDate,
    decimal Salary,
    PersonelInformation PersonelInformation,
    Addrees? Address) : IRequest<Result<string>>;

public sealed class EmployeeCreateCommandValidator : AbstractValidator<EmployeeCreateCommand>
{
    public EmployeeCreateCommandValidator()
    {
        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalı.");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyad alanı en az 3 karakter olmalı.");
        RuleFor(x => x.BirthOfDate).NotEqual(default(DateOnly)).WithMessage("Doğum tarihi boş olamaz.");
        RuleFor(x => x.Salary).GreaterThan(0).WithMessage("Maaş sıfırdan büyük olmalıdır.");
        RuleFor(x => x.PersonelInformation.TCNo)
            .MinimumLength(11).WithMessage("Geçerli bir TC olmalı.")
            .MaximumLength(11).WithMessage("TGeçerli bir TC olmalı.");
    }
}

internal sealed class EmployeeCreateHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EmployeeCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EmployeeCreateCommand request, CancellationToken cancellationToken)
    {
        var isEmloyeeExists = await employeeRepository.AnyAsync(x => x.PersonalInformation.TCNo == request.PersonelInformation.TCNo, cancellationToken);

        if (isEmloyeeExists)
        {
            return Result<string>.Failure("Bu TC Kimlik Numarasına sahip personel zaten mevcut.");
        }

        Employee employee = request.Adapt<Employee>();

        employeeRepository.Add(employee);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Personel başarıyla eklendi.";
    }
}

