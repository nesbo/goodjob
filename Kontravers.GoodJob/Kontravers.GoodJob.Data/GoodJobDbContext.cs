using Microsoft.EntityFrameworkCore;

namespace Kontravers.GoodJob.Data;

public class GoodJobDbContext : DbContext
{
    public GoodJobDbContext() { }
    
    public GoodJobDbContext(DbContextOptions options) : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // configure postgres
            optionsBuilder
                .UseNpgsql("Host=localhost;Database=fleetstep.connect.teltonika;Username=postgres;Password=Password1!");
        }
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GoodJobDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}