using DFCStats.Web.Models.Nationalities;
using FluentValidation;

public class EditNationalityValidation : BaseNationalityValidator<EditNationality>
{
    public EditNationalityValidation()
    {
        // if Id is string
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .NotEqual(Guid.Empty).WithMessage("Id is required");
    }
}