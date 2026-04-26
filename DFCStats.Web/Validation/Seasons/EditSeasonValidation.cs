using DFCStats.Web.Models.Seasons;
using FluentValidation;

namespace DFCStats.Web.Validation.Seasons
{
    public class EditSeasonValidation : AbstractValidator<EditSeason>
    {
        public EditSeasonValidation()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Season description is required")
                .MaximumLength(20).WithMessage("Season description must be 20 characters or less");
        }

    }
}