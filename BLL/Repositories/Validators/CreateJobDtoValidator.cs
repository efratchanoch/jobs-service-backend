using FluentValidation;
using jobs_service_backend.DTOs.Jobs;

namespace jobs_service_backend.BLL.Validators
{
    public class CreateJobDtoValidator : AbstractValidator<CreateJobDto>
    {
        public CreateJobDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("כותרת המשרה היא שדה חובה.")
                                 .MaximumLength(100).WithMessage("הכותרת ארוכה מדי.");
            
            RuleFor(x => x.CompanyName).NotEmpty().WithMessage("שם החברה הוא שדה חובה.");
            
            RuleFor(x => x.Experience).GreaterThanOrEqualTo(0).WithMessage("שנות ניסיון לא יכולות להיות שליליות.");
            
            RuleFor(x => x.Deadline).GreaterThan(DateTime.UtcNow).WithMessage("תאריך סיום חייב להיות בעתיד.");
            
            RuleFor(x => x.SalaryMax)
                .GreaterThan(x => x.SalaryMin).When(x => x.SalaryMin.HasValue && x.SalaryMax.HasValue)
                .WithMessage("שכר מקסימלי חייב להיות גדול מהשכר המינימלי.");
        }
    }
}