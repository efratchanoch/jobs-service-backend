using FluentValidation;
using jobs_service_backend.DTOs;

namespace jobs_service_backend.BLL.Validators;

public class UpdateNotesDtoValidator : AbstractValidator<UpdateNotesDto>
{
    public UpdateNotesDtoValidator()
    {
        RuleFor(x => x.Notes)
            .MaximumLength(10_000)
            .When(x => x.Notes != null)
            .WithMessage("Notes must not exceed 10,000 characters.");
    }
}
