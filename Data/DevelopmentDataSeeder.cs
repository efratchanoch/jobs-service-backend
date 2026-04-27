using jobs_service_backend.Data.Entities;
using jobs_service_backend.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace jobs_service_backend.Data;

public static class DevelopmentDataSeeder
{
    private const string SeedCompanyPrefix = "[DEV-SEED]";
    private const string SeedTagPrefix = "dev-seed:";

    public static async Task SeedIfNeededAsync(AppDbContext dbContext, ILogger logger, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        var hasSeededJobs = await dbContext.Jobs.AnyAsync(j => j.CompanyName.StartsWith(SeedCompanyPrefix), cancellationToken);
        if (hasSeededJobs)
        {
            logger.LogInformation("Development seed already exists. Skipping seed.");
            return;
        }

        await SeedAsync(dbContext, logger, cancellationToken);
    }

    public static async Task SeedAsync(AppDbContext dbContext, ILogger logger, CancellationToken cancellationToken = default)
    {
        var tags = new List<Tag>
        {
            new() { TagName = $"{SeedTagPrefix}react" },
            new() { TagName = $"{SeedTagPrefix}dotnet" },
            new() { TagName = $"{SeedTagPrefix}qa" },
            new() { TagName = $"{SeedTagPrefix}devops" },
            new() { TagName = $"{SeedTagPrefix}backend" },
            new() { TagName = $"{SeedTagPrefix}ui" }
        };

        await dbContext.Tags.AddRangeAsync(tags, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var now = DateTime.UtcNow;
        var jobs = new List<Job>
        {
            new()
            {
                Title = "Frontend Developer",
                Description = "Build and maintain React interfaces.",
                CompanyName = $"{SeedCompanyPrefix} BrightPath",
                Location = "פתח תקווה",
                Experience = 2,
                Requirements = "React, JavaScript, teamwork",
                IsRemote = true,
                IsPrivate = false,
                SalaryMin = 16000,
                SalaryMax = 22000,
                CreatedAt = now.AddDays(-7),
                Deadline = now.AddDays(21),
                IsActive = true,
                JobType = JobType.FullTime,
                Field = Field.Development,
                Status = JobStatus.Open,
                Tags = new List<Tag> { tags[0], tags[5] }
            },
            new()
            {
                Title = "QA Analyst",
                Description = "Manual and automation QA for web platform.",
                CompanyName = $"{SeedCompanyPrefix} CodeSeed",
                Location = "בני ברק",
                Experience = 1,
                Requirements = "Test plans, bug tracking",
                IsRemote = false,
                IsPrivate = false,
                SalaryMin = 11000,
                SalaryMax = 15000,
                CreatedAt = now.AddDays(-12),
                Deadline = now.AddDays(14),
                IsActive = true,
                JobType = JobType.FullTime,
                Field = Field.QA,
                Status = JobStatus.Pending,
                Tags = new List<Tag> { tags[2] }
            },
            new()
            {
                Title = "DevOps Engineer",
                Description = "CI/CD and cloud operations ownership.",
                CompanyName = $"{SeedCompanyPrefix} Noya Systems",
                Location = "תל אביב",
                Experience = 3,
                Requirements = "Pipelines, monitoring, Linux",
                IsRemote = true,
                IsPrivate = false,
                SalaryMin = 20000,
                SalaryMax = 28000,
                CreatedAt = now.AddDays(-20),
                Deadline = now.AddDays(7),
                IsActive = true,
                JobType = JobType.FullTime,
                Field = Field.DevOps,
                Status = JobStatus.Closed,
                Tags = new List<Tag> { tags[3], tags[4] }
            },
            new()
            {
                Title = "Backend .NET Internship",
                Description = "Assist backend team in .NET services.",
                CompanyName = $"{SeedCompanyPrefix} Kesher Group",
                Location = "ירושלים",
                Experience = 0,
                Requirements = "Basic C#, SQL fundamentals",
                IsRemote = false,
                IsPrivate = false,
                SalaryMin = 7000,
                SalaryMax = 9000,
                CreatedAt = now.AddDays(-5),
                Deadline = now.AddDays(30),
                IsActive = true,
                JobType = JobType.Internship,
                Field = Field.Development,
                Status = JobStatus.Open,
                Tags = new List<Tag> { tags[1], tags[4] }
            },
            new()
            {
                Title = "Junior Backend Developer",
                Description = "Maintain APIs and integrations in .NET.",
                CompanyName = $"{SeedCompanyPrefix} Galil Tech",
                Location = "חיפה",
                Experience = 1,
                Requirements = "C#, REST APIs, SQL",
                IsRemote = true,
                IsPrivate = false,
                SalaryMin = 12000,
                SalaryMax = 17000,
                CreatedAt = now.AddDays(-9),
                Deadline = now.AddDays(18),
                IsActive = true,
                JobType = JobType.FullTime,
                Field = Field.Development,
                Status = JobStatus.Pending,
                Tags = new List<Tag> { tags[1], tags[4] }
            },
            new()
            {
                Title = "QA Automation Internship",
                Description = "Build Cypress and API automated tests.",
                CompanyName = $"{SeedCompanyPrefix} Lev QA",
                Location = "רמת גן",
                Experience = 0,
                Requirements = "JavaScript basics, attention to details",
                IsRemote = false,
                IsPrivate = false,
                SalaryMin = 6500,
                SalaryMax = 8500,
                CreatedAt = now.AddDays(-3),
                Deadline = now.AddDays(25),
                IsActive = true,
                JobType = JobType.Internship,
                Field = Field.QA,
                Status = JobStatus.Open,
                Tags = new List<Tag> { tags[0], tags[2] }
            }
        };

        await dbContext.Jobs.AddRangeAsync(jobs, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var applications = new List<Application>
        {
            new() { JobId = jobs[0].JobId, StudentId = 1001, AppliedAt = now.AddDays(-6), Status = ApplicationStatus.Pending, Notes = "Strong portfolio." },
            new() { JobId = jobs[0].JobId, StudentId = 1002, AppliedAt = now.AddDays(-4), Status = ApplicationStatus.Interviewed, Notes = "Interview scheduled." },
            new() { JobId = jobs[1].JobId, StudentId = 1003, AppliedAt = now.AddDays(-9), Status = ApplicationStatus.Pending },
            new() { JobId = jobs[2].JobId, StudentId = 1004, AppliedAt = now.AddDays(-16), Status = ApplicationStatus.Rejected, Notes = "Experience mismatch." },
            new() { JobId = jobs[3].JobId, StudentId = 1005, AppliedAt = now.AddDays(-2), Status = ApplicationStatus.Accepted, Notes = "Accepted and onboarding." },
            new() { JobId = jobs[4].JobId, StudentId = 1006, AppliedAt = now.AddDays(-5), Status = ApplicationStatus.Pending, Notes = "Needs technical screening." },
            new() { JobId = jobs[4].JobId, StudentId = 1007, AppliedAt = now.AddDays(-3), Status = ApplicationStatus.Interviewed, Notes = "Interview completed." },
            new() { JobId = jobs[5].JobId, StudentId = 1008, AppliedAt = now.AddDays(-1), Status = ApplicationStatus.Pending }
        };

        await dbContext.Applications.AddRangeAsync(applications, cancellationToken);

        var invitations = new List<PrivateJobInvitation>
        {
            new() { JobId = jobs[0].JobId, StudentId = 2001, InvitedAt = now.AddDays(-3), IsViewed = true },
            new() { JobId = jobs[1].JobId, StudentId = 2002, InvitedAt = now.AddDays(-2), IsViewed = false },
            new() { JobId = jobs[3].JobId, StudentId = 2003, InvitedAt = now.AddDays(-1), IsViewed = false },
            new() { JobId = jobs[4].JobId, StudentId = 2004, InvitedAt = now.AddHours(-18), IsViewed = true },
            new() { JobId = jobs[5].JobId, StudentId = 2005, InvitedAt = now.AddHours(-12), IsViewed = false }
        };

        await dbContext.PrivateJobInvitations.AddRangeAsync(invitations, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Development seed inserted: {Jobs} jobs, {Tags} tags, {Applications} applications, {Invitations} invitations.",
            jobs.Count, tags.Count, applications.Count, invitations.Count);
    }

    public static async Task<int> ClearSeedDataAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var seedJobIds = await dbContext.Jobs
            .Where(j => j.CompanyName.StartsWith(SeedCompanyPrefix))
            .Select(j => j.JobId)
            .ToListAsync(cancellationToken);

        if (seedJobIds.Count == 0)
        {
            var removedTagsOnly = await RemoveSeedTagsAsync(dbContext, cancellationToken);
            return removedTagsOnly;
        }

        var applications = await dbContext.Applications.Where(a => seedJobIds.Contains(a.JobId)).ToListAsync(cancellationToken);
        var invitations = await dbContext.PrivateJobInvitations.Where(i => seedJobIds.Contains(i.JobId)).ToListAsync(cancellationToken);
        var jobs = await dbContext.Jobs.Where(j => seedJobIds.Contains(j.JobId)).ToListAsync(cancellationToken);
        var tagsRemoved = await RemoveSeedTagsAsync(dbContext, cancellationToken);

        dbContext.Applications.RemoveRange(applications);
        dbContext.PrivateJobInvitations.RemoveRange(invitations);
        dbContext.Jobs.RemoveRange(jobs);

        var removedCount = applications.Count + invitations.Count + jobs.Count + tagsRemoved;
        await dbContext.SaveChangesAsync(cancellationToken);
        return removedCount;
    }

    private static async Task<int> RemoveSeedTagsAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var tags = await dbContext.Tags.Where(t => t.TagName.StartsWith(SeedTagPrefix)).ToListAsync(cancellationToken);
        if (tags.Count > 0)
        {
            dbContext.Tags.RemoveRange(tags);
        }

        return tags.Count;
    }
}
