using DFCStats.Web.Models.Nationalities;
using FluentValidation;

public class EditNationalityValidation : AbstractValidator<EditNationality>
{
    public EditNationalityValidation()
    {
        // if Id is string
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .NotEqual(Guid.Empty).WithMessage("Id is required");

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