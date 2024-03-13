using ByAllMeans.Api.Features.Sneakers;
using Microsoft.EntityFrameworkCore;

namespace ByAllMeans.Api.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Sneakers> Sneakers { get; set; }
}