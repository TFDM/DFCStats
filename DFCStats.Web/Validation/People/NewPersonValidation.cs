using DFCStats.Web.Models.People;
using FluentValidation;

namespace DFCStats.Web.Validation.People
{
    public class NewClubValidation : AbstractValidator<NewPerson>
    {
        public NewClubValidation()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name must be 50 characters or less");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name must be 50 characters or less");

            RuleFor(x => x.DateOfBirth)
                .Must(d => !d.HasValue || d.Value <= DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Date of birth must not be in the future.");
        }

    }
}