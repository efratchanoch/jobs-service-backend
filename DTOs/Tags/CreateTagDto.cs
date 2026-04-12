using System.ComponentModel.DataAnnotations;

namespace jobs_service_backend.DTOs.Tags
{
    public class CreateTagDto
    {
        [Required(ErrorMessage = "Tag name is required.")]
        [MaxLength(200)]
        public string TagName { get; set; } = string.Empty;
    }
}
