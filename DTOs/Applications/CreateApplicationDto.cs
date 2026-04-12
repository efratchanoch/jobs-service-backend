namespace jobs_service_backend.DTOs;

public class CreateApplicationDto
{
    public int JobId { get; set; }

    public string? CoverLetter { get; set; }

    /// <summary>כתובת קובץ קורות חיים (למשל מהחזרת <c>POST /api/files/resume</c>).</summary>
    public string? ResumeUrl { get; set; }
}

