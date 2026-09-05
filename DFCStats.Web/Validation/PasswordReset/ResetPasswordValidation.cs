using DFCStats.Web.Models.PasswordResets;
using FluentValidation;

public class ResetPasswordValidation : AbstractValidator<ResetPassword>
{
    public ResetPasswordValidation()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("Password is required.");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty()
                .WithMessage("Password confirmation is required.")
            .Equal(x => x.NewPassword)
                .WithMessage("Passwords must match.");
    }
}