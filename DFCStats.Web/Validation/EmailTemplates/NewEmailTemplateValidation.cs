using DFCStats.Web.Models.EmailTemplates;
using FluentValidation;

namespace DFCStats.Web.Validation.EmailTemplates
{
    public class NewEmailTemplateValidation : AbstractValidator<NewEmailTemplate>
    {
        public NewEmailTemplateValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(50).WithMessage("Title must be 50 characters or less");

            RuleFor(x => x.IsHtml)
                .NotNull().WithMessage("Please select yes or no");

            RuleFor(x => x.Template)
                .NotEmpty().WithMessage("Template is required");
        }

    }
}