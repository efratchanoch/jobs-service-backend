using Microsoft.EntityFrameworkCore;
using jobs_service_backend.Data.Entities;

namespace jobs_service_backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Job> Jobs { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<Application> Applications { get; set; } = null!;
    public DbSet<PrivateJobInvitation> PrivateJobInvitations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Job>()
            .Property(j => j.IsActive)
            .IsRequired();

        modelBuilder.Entity<Job>()
            .HasMany(j => j.Tags)
            .WithMany(t => t.Jobs)
            .UsingEntity(join => join.ToTable("JobTags"));

        modelBuilder.Entity<Application>()
            .HasIndex(a => new { a.JobId, a.StudentId })
            .IsUnique();

        modelBuilder.Entity<Application>()
            .HasOne(a => a.Job)
            .WithMany()
            .HasForeignKey(a => a.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PrivateJobInvitation>()
            .HasOne(i => i.Job)
            .WithMany()
            .HasForeignKey(i => i.JobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

