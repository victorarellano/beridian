namespace Beridian.Infrastructure.Tests.Persistence;

[CollectionDefinition(Name)]
public sealed class PostgreSqlCollection
    : ICollectionFixture<PostgreSqlFixture>
{
    public const string Name = "PostgreSQL";
}