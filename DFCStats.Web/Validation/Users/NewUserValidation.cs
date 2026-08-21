using DFCStats.Web.Models.Users;
using FluentValidation;

public class NewUserValidation : AbstractValidator<NewUser>
{
    public NewUserValidation()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
                .WithMessage("Password confirmation is required.")
            .Equal(x => x.Password)
                .WithMessage("Passwords must match.");

        RuleFor(x => x.AllowLogin)
            .NotNull().WithMessage("Please select yes or no");
    }
}