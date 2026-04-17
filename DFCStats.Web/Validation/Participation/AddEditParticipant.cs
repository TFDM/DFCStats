using DFCStats.Web.Models.Participants;
using FluentValidation;

public class AddEditParticipantValidator : AbstractValidator<AddEditParticipant>
{
    public AddEditParticipantValidator()
    {
        RuleFor(x => x.FixtureId)
            .NotEmpty().WithMessage("Fixture is required");

        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("Person is required");

        RuleFor(x => x.RoleInFixture)
            .NotEmpty().WithMessage("Role in Fixture is required");

        RuleFor(x => x.Goals)
            .GreaterThanOrEqualTo(0).WithMessage("Goals cannot be negative");

        RuleFor(x => x.YellowCard)
            .NotNull().WithMessage("You must specify if a yellow card was given");

        RuleFor(x => x.RedCard)
            .NotNull().WithMessage("You must specify if a red card was given");
        
        // RuleFor(x => x.ReplacedTime)
        //     .InclusiveBetween(1, 130).When(x => x.ReplacedTime.HasValue)
        //     .WithMessage("Replaced Time must be a valid match minute");

        // RuleFor(x => x.ReplacedByPersonId)
        //     .NotEmpty().When(x => x.ReplacedTime.HasValue)
        //     .WithMessage("If a replacement time is set, you must specify who replaced them");


        RuleFor(x => x.ReplacedTime)
    .NotNull()
    .When(x => x.ReplacedByPersonId != null)
    .WithMessage("Replaced Time is required when a replacement player is selected")
    .DependentRules(() =>
    {
        RuleFor(x => x.ReplacedTime)
            .InclusiveBetween(1, 130)
            .WithMessage("Replaced Time must be a valid match minute");
    });



RuleFor(x => x.ReplacedByPersonId)
    .NotEmpty()
    .When(x => x.ReplacedTime != null)
    .WithMessage("If a replacement time is set, you must specify who replaced them");

// Optional (better UX consistency)
RuleFor(x => x.ReplacedTime)
    .NotNull()
    .When(x => x.ReplacedByPersonId != null)
    .WithMessage("If a replacement player is selected, you must enter a time");
    }
}