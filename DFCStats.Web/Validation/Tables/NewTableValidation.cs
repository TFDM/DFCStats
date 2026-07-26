using DFCStats.Web.Models.Tables;
using FluentValidation;

public class NewTableValidation : AbstractValidator<NewTable>
{
    public NewTableValidation()
    {
        RuleFor(x => x.ClubId)
            .NotEmpty()
            .Unless(x => x.IsDarlington ?? false).WithMessage("Club is required");

        RuleFor(x => x.Played)
            .NotEmpty().WithMessage("Games played is required")
            .GreaterThan(0).WithMessage("Games played must be greater than 0");

        RuleFor(x => x.HomeWon)
            .NotEmpty().WithMessage("Home games won is required")
            .GreaterThanOrEqualTo(0).WithMessage("Home games won must be greater than or equal to 0");

        RuleFor(x => x.HomeDrawn)
            .NotEmpty().WithMessage("Home games drawn is required")
            .GreaterThanOrEqualTo(0).WithMessage("Home games drawn must be greater than or equal to 0");

        RuleFor(x => x.HomeLost)
            .NotEmpty().WithMessage("Home games lost is required")
            .GreaterThanOrEqualTo(0).WithMessage("Home games lost must be greater than or equal to 0");

        RuleFor(x => x.HomeGoalsFor)
            .NotEmpty().WithMessage("Home goals for is required")
            .GreaterThanOrEqualTo(0).WithMessage("Home goals for must be greater than or equal to 0");

        RuleFor(x => x.HomeGoalsAgainst)
            .NotEmpty().WithMessage("Home goals against is required")
            .GreaterThanOrEqualTo(0).WithMessage("Home goals against must be greater than or equal to 0");

        RuleFor(x => x.AwayWon)
            .NotEmpty().WithMessage("Away games won is required")
            .GreaterThanOrEqualTo(0).WithMessage("Away games won must be greater than or equal to 0");

        RuleFor(x => x.AwayDrawn)
            .NotEmpty().WithMessage("Away games drawn is required")
            .GreaterThanOrEqualTo(0).WithMessage("Away games drawn must be greater than or equal to 0");

        RuleFor(x => x.AwayLost)
            .NotEmpty().WithMessage("Away games lost is required")
            .GreaterThanOrEqualTo(0).WithMessage("Away games lost must be greater than or equal to 0");

        RuleFor(x => x.AwayGoalsFor)
            .NotEmpty().WithMessage("Away goals for is required")
            .GreaterThanOrEqualTo(0).WithMessage("Away goals for must be greater than or equal to 0");

        RuleFor(x => x.AwayGoalsAgainst)
            .NotEmpty().WithMessage("Away goals against is required")
            .GreaterThanOrEqualTo(0).WithMessage("Away goals against must be greater than or equal to 0");

        RuleFor(x => x.Points)
            .NotEmpty().WithMessage("Points is required")
            .GreaterThanOrEqualTo(0).WithMessage("Points must be greater than or equal to 0");

        RuleFor(x => x.IsChampion)
            .NotNull().WithMessage("Please select yes or no");

        RuleFor(x => x.IsPromotion)
            .NotNull().WithMessage("Please select yes or no");

        RuleFor(x => x.IsPlayOffs)
            .NotNull().WithMessage("Please select yes or no");

        RuleFor(x => x.IsRelegated)
            .NotNull().WithMessage("Please select yes or no");

        RuleFor(x => x.IsDarlington)
            .NotNull().WithMessage("Please select yes or no");
    }

}