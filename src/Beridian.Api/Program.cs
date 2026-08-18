using Beridian.Api.Endpoints.FinancialPeriods;
using Beridian.Api.ExceptionHandling;
using Beridian.Api.OpenApi;
using Beridian.Api.Versioning;
using Beridian.Application;
using Beridian.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//Add services from layers
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddApiVersioningConfiguration();   

// Add services to the container.
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services
    .AddProblemDetails()
    .AddAllExceptionHandlers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var apiVersionDescriptionProvider =
            app.Services.GetRequiredService<Asp.Versioning.ApiExplorer.IApiVersionDescriptionProvider>();

        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                $"Beridian API {description.GroupName.ToUpperInvariant()}");
        }
    });
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapFinancialPeriodEndpoints();

app.Run();

