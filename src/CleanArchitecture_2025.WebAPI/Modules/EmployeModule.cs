﻿using CleanArchitecture_2025.Application.Employees;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.WebAPI.Modules;

public static class EmployeModule
{
    public static void RegisterEmployeeRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/employees").WithTags("Employees");

        group.MapPost(string.Empty,
            async (ISender sender, EmployeeCreateCommand request, CancellationToken cancellationToken) =>
            {

                var response = await sender.Send(request, cancellationToken);

                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }
}
