using Asp.Versioning;

namespace Beridian.Api.Versioning;

public static class ApiVersions
{
    public static ApiVersion V1 { get; } = new(1, 0);
}