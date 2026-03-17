using System.Security.Claims;

namespace jobs_service_backend.BLL.Services;

public interface IIdentityService
{
    int GetStudentId(ClaimsPrincipal user);
}

