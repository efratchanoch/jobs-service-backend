using System.Security.Claims;

namespace jobs_service_backend.BLL.Services;

public class IdentityService : IIdentityService
{
    private const string StudentIdClaimType = "StudentId";

    public int GetStudentId(ClaimsPrincipal user)
    {
        if (user == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var value =
            user.FindFirstValue(StudentIdClaimType)
            ?? user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(value) || !int.TryParse(value, out var id))
        {
            throw new UnauthorizedAccessException("Student ID is missing or invalid in the token.");
        }

        return id;
    }
}

