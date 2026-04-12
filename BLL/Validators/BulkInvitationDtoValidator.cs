using FluentValidation;
using jobs_service_backend.DTOs.Invitations;

namespace jobs_service_backend.BLL.Validators;

public class BulkInvitationDtoValidator : AbstractValidator<BulkInvitationDto>
{
    public BulkInvitationDtoValidator()
    {
        RuleFor(x => x.JobId)
            .GreaterThan(0)
            .WithMessage("Job ID must be a positive number.");

        RuleFor(x => x.StudentIds)
            .NotEmpty()
            .WithMessage("Student list is required.");

        RuleForEach(x => x.StudentIds)
            .GreaterThan(0)
            .WithMessage("Each student ID must be a positive number.");
    }
}
