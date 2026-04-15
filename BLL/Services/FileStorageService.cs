using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace jobs_service_backend.BLL.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;

    private static readonly string[] JobImageExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    private static readonly string[] JobImageContentTypes =
    [
        "image/jpeg", "image/png", "image/webp", "image/gif"
    ];

    private static readonly string[] ResumeExtensions = [".pdf", ".doc", ".docx"];
    private static readonly string[] ResumeContentTypes =
    [
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    ];

    public FileStorageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveJobImageAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        ValidateFile(file, maxBytes: 5 * 1024 * 1024, JobImageExtensions, JobImageContentTypes);
        return await SaveToSubfolderAsync(file, "jobs", cancellationToken);
    }

    public async Task<string> SaveResumeAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        ValidateFile(file, maxBytes: 15 * 1024 * 1024, ResumeExtensions, ResumeContentTypes);
        var dir = PrivateResumesDirectory;
        Directory.CreateDirectory(dir);

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid():N}{ext}";
        var physicalPath = Path.Combine(dir, fileName);

        await using (var stream = File.Create(physicalPath))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return fileName;
    }

    public string? GetResumePhysicalPath(string fileName)
    {
        fileName = Path.GetFileName(fileName);
        if (string.IsNullOrEmpty(fileName)) return null;

        var path = Path.Combine(PrivateResumesDirectory, fileName);
        return File.Exists(path) ? path : null;
    }

    private static string ResolvePrivateResumesDirectory(IWebHostEnvironment env)
        => Path.Combine(env.ContentRootPath, "private-uploads", "resumes");

    private string PrivateResumesDirectory => ResolvePrivateResumesDirectory(_env);

    private string WebRootPath
    {
        get
        {
            var root = _env.WebRootPath;
            if (string.IsNullOrEmpty(root))
                root = Path.Combine(_env.ContentRootPath, "wwwroot");
            Directory.CreateDirectory(root);
            return root;
        }
    }

    private static void ValidateFile(IFormFile file, long maxBytes, string[] allowedExt, string[] allowedContentTypes)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("קובץ נדרש.");

        if (file.Length > maxBytes)
            throw new ArgumentException($"הקובץ גדול מדי (מקסימום {maxBytes / 1024 / 1024}MB).");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(ext) || !allowedExt.Contains(ext))
            throw new ArgumentException("סוג הקובץ אינו נתמך.");

        var ct = file.ContentType?.ToLowerInvariant() ?? "";
        if (!IsAllowedContentType(ct, ext, allowedContentTypes))
            throw new ArgumentException("סוג התוכן (Content-Type) אינו נתמך.");
    }

    private async Task<string> SaveToSubfolderAsync(IFormFile file, string subfolder, CancellationToken cancellationToken)
    {
        var uploads = Path.Combine(WebRootPath, "uploads", subfolder);
        Directory.CreateDirectory(uploads);

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid():N}{ext}";
        var physicalPath = Path.Combine(uploads, fileName);

        await using (var stream = File.Create(physicalPath))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return $"/uploads/{subfolder}/{fileName}";
    }

    private static bool IsAllowedContentType(string ct, string ext, string[] allowedContentTypes)
    {
        if (allowedContentTypes.Contains(ct)) return true;
        // Extension is already validated; browsers often send generic octet-stream for uploads.
        if (ct == "application/octet-stream") return true;
        return false;
    }
}
