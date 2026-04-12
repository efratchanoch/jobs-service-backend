using FluentValidation;
using jobs_service_backend.DTOs;

namespace jobs_service_backend.BLL.Validators;

public class CreateApplicationDtoValidator : AbstractValidator<CreateApplicationDto>
{
    public CreateApplicationDtoValidator()
    {
        RuleFor(x => x.JobId)
            .GreaterThan(0)
            .WithMessage("JobId is required.");

        RuleFor(x => x.CoverLetter)
            .MaximumLength(20_000)
            .When(x => !string.IsNullOrEmpty(x.CoverLetter))
            .WithMessage("Cover letter is too long.");

        RuleFor(x => x.ResumeUrl)
            .Must(HttpUrlRules.BeValidOptionalHttpUrl)
            .When(x => !string.IsNullOrWhiteSpace(x.ResumeUrl))
            .WithMessage("ResumeUrl must be a valid http or https URL.");
    }
}
