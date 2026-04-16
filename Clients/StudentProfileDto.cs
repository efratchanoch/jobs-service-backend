namespace jobs_service_backend.Clients;

/// <summary>
/// DTO representing the response from the student profile microservice (student_profile).
/// Matches the object returned from GET /api/student/{id}.
/// </summary>
public class StudentProfileDto
{
    // public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    // public string Bio { get; set; } = string.Empty;
    // public List<string> Skills { get; set; } = new();
}
