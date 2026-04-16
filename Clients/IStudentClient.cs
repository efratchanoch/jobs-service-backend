namespace jobs_service_backend.Clients;

/// <summary>
/// Contract for the HTTP client that communicates with the student profile microservice.
/// </summary>
public interface IStudentClient
{
    /// <summary>
    /// Fetches a student profile by its unique identifier (UserId) from the profile microservice.
    /// </summary>
    /// <param name="studentId">The student's unique identifier (Guid).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="StudentProfileDto"/> if found, otherwise null.</returns>
    Task<StudentProfileDto?> GetStudentByIdAsync(Guid studentId, CancellationToken cancellationToken = default);
}
