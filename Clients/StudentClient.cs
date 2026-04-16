using System.Net;
using System.Net.Http.Json;

namespace jobs_service_backend.Clients;

/// <summary>
/// Implementation of <see cref="IStudentClient"/> using an injected <see cref="HttpClient"/>.
/// The HttpClient receives its BaseAddress from Program.cs (AddHttpClient).
/// </summary>
public class StudentClient : IStudentClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StudentClient> _logger;

    public StudentClient(HttpClient httpClient, ILogger<StudentClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<StudentProfileDto?> GetStudentByIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"api/student/{studentId}";

        try
        {
            var response = await _httpClient.GetAsync(requestUri, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<StudentProfileDto>(cancellationToken: cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling the profile microservice for student {StudentId}", studentId);
            throw;
        }
    }
}
