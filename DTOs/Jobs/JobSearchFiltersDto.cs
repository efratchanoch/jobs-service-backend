using JobsService.Data.Enums;

namespace JobsService.DTOs.Jobs
{
    public class JobSearchFiltersDto
    {
        public string? FreeText { get; set; }
        public string? Location { get; set; }
        public Field? Field { get; set; }
        public List<int>? TagIds { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}