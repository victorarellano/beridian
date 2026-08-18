using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Beridian.Api.OpenApi;

public sealed class ConfigureSwaggerOptions
    : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, 
                new OpenApiInfo
                {
                    Title = "Beridian API",
                    Version = description.ApiVersion.ToString(),
                    Description = "Personal financial planning API."
                });
        }
    }
}