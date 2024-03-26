using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kontravers.GoodJob.Auth.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
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
}