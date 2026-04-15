using FluentValidation;
using jobs_service_backend.DTOs.Jobs;

namespace jobs_service_backend.BLL.Validators
{
    public class UpdateJobDtoValidator : AbstractValidator<UpdateJobDto>
    {
        public UpdateJobDtoValidator()
        {
            RuleFor(x => x.JobWebsiteUrl)
                .Must(HttpUrlRules.BeValidOptionalHttpUrl)
                .When(x => !string.IsNullOrWhiteSpace(x.JobWebsiteUrl))
                .WithMessage("JobWebsiteUrl must be a valid http or https URL.");

            RuleFor(x => x.JobImageUrl)
                .Must(HttpUrlRules.BeValidOptionalHttpUrl)
                .When(x => !string.IsNullOrWhiteSpace(x.JobImageUrl))
                .WithMessage("JobImageUrl must be a valid http or https URL.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Job title is required.")
                .MaximumLength(100).WithMessage("Title is too long.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Job description is required.");

            RuleFor(x => x.Requirements)
                .NotEmpty().WithMessage("Requirements field is required.");

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