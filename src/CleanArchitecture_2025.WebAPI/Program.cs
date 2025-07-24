using CleanArchitecture_2025.Application;
using CleanArchitecture_2025.Infrastructure;
using CleanArchitecture_2025.WebAPI.Controllers;
using CleanArchitecture_2025.WebAPI.Modules;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;

namespace CleanArchitecture_2025.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddCors();
        builder.Services.AddOpenApi();
        builder.Services.AddControllers().AddOData(opt=>
        opt
        .Select()
        .Expand()
        .Filter()
        .OrderBy()
        .Count()
        .SetMaxTop(null)
        .AddRouteComponents("odata", AppOdataController.GetEdmModel())
        );
        builder.Services.AddRateLimiter(x =>
        x.AddSlidingWindowLimiter("fixed", cfg =>
        {
            cfg.QueueLimit = 100;
            cfg.Window = TimeSpan.FromSeconds(1);
            cfg.PermitLimit = 100;
            cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        }));

        builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

        builder.Services.AddAuthentication().AddBearerToken();
        builder.Services.AddAuthorization();
        builder.Services.AddResponseCompression();

        var app = builder.Build();

        app.MapOpenApi();
        app.MapScalarApiReference();

        app.UseHttpsRedirection();


        app.UseCors(policy => policy
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(t=>true));

        app.RegisterRoutes();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseResponseCompression();

        app.UseExceptionHandler();

        app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization(); ;

        ExtensionsMiddleware.CreateFirstUser(app);

        app.Run();
    }
}

