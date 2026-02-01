using DFCStats.Web.Models.Clubs;
using FluentValidation;

namespace DFCStats.Web.Validation.Clubs
{
    public class NewClubValidation : AbstractValidator<NewClub>
    {
        public NewClubValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Club name is required")
                .MaximumLength(50).WithMessage("Club name must be 50 characters or less");
        }

    }
}