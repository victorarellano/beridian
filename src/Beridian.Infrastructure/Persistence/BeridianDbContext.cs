using Beridian.Domain.FinancialPeriods;
using Microsoft.EntityFrameworkCore;

namespace Beridian.Infrastructure.Persistence;

public sealed class BeridianDbContext : DbContext
{
    public BeridianDbContext(DbContextOptions<BeridianDbContext> options)
        : base(options)
    {
    }

    public DbSet<FinancialPeriod> FinancialPeriods =>
        Set<FinancialPeriod>();    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BeridianDbContext).Assembly);
    }
}