using DFCStats.Web.Models.Users;
using FluentValidation;

public class EditUserValidation : AbstractValidator<EditUser>
{
    public EditUserValidation()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.AllowLogin)
            .NotNull().WithMessage("Please select yes or no");
    }
}