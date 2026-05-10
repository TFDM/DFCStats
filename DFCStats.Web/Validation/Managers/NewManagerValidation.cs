using DFCStats.Web.Models.Managers;
using FluentValidation;

public class NewManagerValidation : AbstractValidator<NewManager>
{
    public NewManagerValidation()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("Manager is required");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .Must(d => d.HasValue && d.Value <= DateOnly.FromDateTime(DateTime.Today)).WithMessage("Start date must not be in the future");

        RuleFor(x => x.EndDate)
            .Must(d => !d.HasValue || d.Value <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("End date must not be in the future");

        // Ensure StartDate is before EndDate
        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("Start date must be before or equal to the end date");

        // Ensure EndDate is after StartDate
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("End date cannot be earlier than the start date");

        RuleFor(x => x.IsCaretaker)
            .NotNull().WithMessage("Caretaker status is required");
    }
}