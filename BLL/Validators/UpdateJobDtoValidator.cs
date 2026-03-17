using FluentValidation;
using jobs_service_backend.DTOs.Jobs;

namespace jobs_service_backend.BLL.Validators
{
    public class UpdateJobDtoValidator : AbstractValidator<UpdateJobDto>
    {
        public UpdateJobDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Job title is required.")
                .MaximumLength(100).WithMessage("Title is too long.");

            RuleFor(x => x.Experience)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Experience must be a non-negative integer.");

            RuleFor(x => x.Deadline)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.Deadline.HasValue)
                .WithMessage("Deadline must be in the future.");
        }
    }
}