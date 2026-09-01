using DFCStats.Web.Models.PasswordResets;
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