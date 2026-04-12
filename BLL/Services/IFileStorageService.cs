using Microsoft.AspNetCore.Http;

namespace jobs_service_backend.BLL.Services;

public interface IFileStorageService
{
    /// <summary>Saves a job listing image under wwwroot/uploads/jobs. Returns relative URL path starting with /uploads/jobs/</summary>
    Task<string> SaveJobImageAsync(IFormFile file, CancellationToken cancellationToken = default);

    /// <summary>Saves a resume under ContentRoot/private-uploads/resumes (not web-served). Returns the stored file name only.</summary>
    Task<string> SaveResumeAsync(IFormFile file, CancellationToken cancellationToken = default);

    /// <summary>Resolves the absolute path to a stored resume file, or null if missing or invalid.</summary>
    string? GetResumePhysicalPath(string fileName);
}
