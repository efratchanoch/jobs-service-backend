using FluentValidation;
using jobs_service_backend.DTOs;

namespace jobs_service_backend.BLL.Validators;

public class UpdateApplicationStatusDtoValidator : AbstractValidator<UpdateApplicationStatusDto>
{
    public UpdateApplicationStatusDtoValidator()
    {
        RuleFor(x => x.ApplicationId)
            .GreaterThan(0)
            .WithMessage("ApplicationId is required.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status must be a valid ApplicationStatus value.");

        RuleFor(x => x.Notes)
            .MaximumLength(10_000)
            .When(x => x.Notes != null)
            .WithMessage("Notes must not exceed 10,000 characters.");
    }
}
