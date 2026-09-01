using DFCStats.Web.Models.Users;
using FluentValidation;

public class ForgotPasswordValidation : AbstractValidator<ForgotPassword>
{
    public ForgotPasswordValidation()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress();
    }
}