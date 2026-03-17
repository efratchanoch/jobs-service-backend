using FluentValidation;
using jobs_service_backend.DTOs.Jobs;

namespace jobs_service_backend.BLL.Validators
{
    public class CreateJobDtoValidator : AbstractValidator<CreateJobDto>
    {
        public CreateJobDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Job title is required.")
                .MaximumLength(100).WithMessage("Title is too long.");
            
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required.");
            
            RuleFor(x => x.Experience)
                .NotEmpty().WithMessage("Years of experience is required.")
                .Must(value =>
                {
                    if (!int.TryParse(value, out var years)) return false;
                    return years >= 0;
                })
                .WithMessage("Experience must be a non-negative integer.");
            
            RuleFor(x => x.Deadline)
                .GreaterThan(DateTime.UtcNow).WithMessage("Deadline must be in the future.");
            
            RuleFor(x => x.SalaryMax)
                .GreaterThan(x => x.SalaryMin)
                .When(x => x.SalaryMin.HasValue && x.SalaryMax.HasValue)
                .WithMessage("Maximum salary must be greater than minimum salary.");
        }
    }
}