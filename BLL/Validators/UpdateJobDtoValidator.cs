using FluentValidation;
using jobs_service_backend.DTOs.Jobs;

namespace jobs_service_backend.BLL.Validators
{
    public class UpdateJobDtoValidator : AbstractValidator<UpdateJobDto>
    {
        public UpdateJobDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("כותרת המשרה היא שדה חובה.")
                .MaximumLength(100).WithMessage("הכותרת ארוכה מדי.");

            RuleFor(x => x.Experience)
                .GreaterThanOrEqualTo(0)
                .WithMessage("שנות ניסיון חייב להיות מספר שלם גדול או שווה לאפס.");

            RuleFor(x => x.Deadline)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.Deadline.HasValue)
                .WithMessage("תאריך סיום חייב להיות בעתיד.");
        }
    }
}

