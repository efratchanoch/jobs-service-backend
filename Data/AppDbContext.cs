using Microsoft.EntityFrameworkCore;

namespace jobs_service_backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}

