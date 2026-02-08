using DFCStats.Web.Models.Nationalities;
using FluentValidation;

public abstract class BaseNationalityValidator<T> : AbstractValidator<T> where T : INationality
{
    /// <summary>
    /// Defines a base set of validation rules for both the NewNationality and EditNationality models. 
    /// These rules are applied to both models and can be extended in the specific validators for each model.
    /// </summary>
    protected BaseNationalityValidator()
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