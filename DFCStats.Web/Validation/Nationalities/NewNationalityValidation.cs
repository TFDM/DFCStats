using DFCStats.Web.Models.Nationalities;
using FluentValidation;

public class NewNationalityValidation : AbstractValidator<NewNationality>
{
    public NewNationalityValidation()
    {
        RuleFor(x => x.Nationality)
            .NotEmpty().WithMessage("Nationality is required")
            .MaximumLength(50).WithMessage("Nationality must be 50 characters or less");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(50).WithMessage("Country must be 50 characters or less");

        RuleFor(x => x.Icon)
            .MaximumLength(10).WithMessage("Icon URL must be 10 characters or less");
    }
}