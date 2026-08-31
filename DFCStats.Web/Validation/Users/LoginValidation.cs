using DFCStats.Web.Models.Users;
using FluentValidation;

public class LoginValidation : AbstractValidator<Login>
{
    public LoginValidation()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .NotNull().WithMessage("Please enter your password");
    }
}