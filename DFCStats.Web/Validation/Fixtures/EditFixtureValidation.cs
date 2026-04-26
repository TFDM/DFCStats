using DFCStats.Web.Models.Fixtures;
using FluentValidation;

public class EditFixtureValidation : AbstractValidator<EditFixture>
{
    public EditFixtureValidation()
    {
        RuleFor(x => x.SeasonId)
            .NotEmpty().WithMessage("Season is required");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is requiredxxx");

        RuleFor(x => x.ClubId)
            .NotEmpty().WithMessage("Club is requiredxxxx");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required");
        
        RuleFor(x => x.Competition)
            .NotEmpty().WithMessage("Competition is required")
            .MaximumLength(50).WithMessage("Competition must be 50 characters or less");

        RuleFor(x => x.VenueId)
            .NotEmpty().WithMessage("Venue is required");

        RuleFor(x => x.DarlingtonScore)
            .GreaterThanOrEqualTo(0).WithMessage("Only positive numbers allowed");

        RuleFor(x => x.OppositionScore)
            .GreaterThanOrEqualTo(0).WithMessage("Only positive numbers allowed");

        RuleFor(x => x.DarlingtonPenaltyScore)
            .NotNull().WithMessage("Darlington Penalty Score is required when penalties are required.")
            .GreaterThanOrEqualTo(0).WithMessage("Only positive numbers allowed")
            .When(x => x.PenaltiesRequired);

        RuleFor(x => x.OppositionPenaltyScore)
            .NotNull().WithMessage("Opposition Penalty Score is required when penalties are required.")
            .GreaterThanOrEqualTo(0).WithMessage("Only positive numbers allowed")
            .When(x => x.PenaltiesRequired);

        RuleFor(x => x.Attendance)
            .GreaterThanOrEqualTo(0).When(x => x.Attendance.HasValue);
    }
}