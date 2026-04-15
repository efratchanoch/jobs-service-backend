namespace jobs_service_backend.DTOs.Common;

/// <summary>
/// Shared rules for query paging across API and services.
/// </summary>
public static class Pagination
{
    public const int DefaultPageSize = 10;
    public const int MinPageSize = 1;
    public const int MaxPageSize = 100;

    /// <summary>
    /// Clamps page index to at least 1 and page size to <see cref="MinPageSize"/>–<see cref="MaxPageSize"/>.
    /// Invalid or missing sizes (e.g. 0) fall back to <see cref="DefaultPageSize"/>.
    /// </summary>
    public static (int PageNumber, int PageSize) Normalize(int pageNumber, int pageSize)
    {
        var pn = pageNumber < 1 ? 1 : pageNumber;
        var ps = pageSize < MinPageSize ? DefaultPageSize : pageSize;
        if (ps > MaxPageSize) ps = MaxPageSize;
        return (pn, ps);
    }
}
