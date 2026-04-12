namespace jobs_service_backend.DTOs.Invitations
{
    /// <summary>
    /// API representation of a private job invitation shown to the invited student.
    /// </summary>
    /// <remarks>
    /// Populated from the private-invitation persistence model via AutoMapper. Use <see cref="IsViewedByStudent"/> to split
    /// &quot;new&quot; vs historical items on the client, or call <c>GET /api/Invitations/my/new</c> for server-filtered new items only.
    /// </remarks>
    public class InvitationDto
    {
        /// <summary>Primary key of the invitation row.</summary>
        public int InvitationId { get; set; }

        /// <summary>Referenced job identifier.</summary>
        public int JobId { get; set; }

        /// <summary>Display title from the related job.</summary>
        public string JobTitle { get; set; } = string.Empty;

        /// <summary>UTC timestamp when the invitation was created.</summary>
        public DateTime InvitedAt { get; set; }

        /// <summary>
        /// When <c>false</c>, the invitation is treated as &quot;new&quot; until the student calls the mark-viewed endpoint.
        /// </summary>
        public bool IsViewedByStudent { get; set; }
    }
}
