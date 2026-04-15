namespace jobs_service_backend.BLL.Validators;

internal static class HttpUrlRules
{
    /// <summary>Returns true for null/whitespace, or an absolute http/https URL.</summary>
    public static bool BeValidOptionalHttpUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return true;
        return Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
