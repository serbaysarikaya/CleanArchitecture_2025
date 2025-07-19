using CleanArchitecture_2025.Application.Employees;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace CleanArchitecture_2025.WebAPI.Controllers;

[Route("odata")]
[ApiController]
[EnableQuery]
public class AppOdataController(ISender sender):ODataController
{
  
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase(); 
        builder.EntitySet<EmployeeGetAllQueryResponse>("Employees");
        return builder.GetEdmModel();
    }

    [HttpGet("employees")]
  
    public async Task<IQueryable<EmployeeGetAllQueryResponse>> GetEmployeesAsync(CancellationToken cancellationToken)
    {

        var response = await sender.Send(new EmployeeGetAllQuery(), cancellationToken);

        return response;

    }

}
