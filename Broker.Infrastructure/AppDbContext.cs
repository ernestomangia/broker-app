using Broker.Domain;
using Microsoft.EntityFrameworkCore;

namespace Broker.Infrastructure;

public class AppDbContext : DbContext
{
    #region Constructor

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Rate> Rates { get; set; }
    public DbSet<RateValue> RateValues { get; set; }
}