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

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.");

            RuleFor(x => x.SalaryMin)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SalaryMin.HasValue)
                .WithMessage("Minimum salary cannot be negative.");

            RuleFor(x => x.SalaryMax)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SalaryMax.HasValue)
                .WithMessage("Maximum salary cannot be negative.");

            RuleFor(x => x.SalaryMax)
                .GreaterThan(x => x.SalaryMin)
                .When(x => x.SalaryMin.HasValue && x.SalaryMax.HasValue)
                .WithMessage("Maximum salary must be greater than minimum salary.");
        }
    }
}