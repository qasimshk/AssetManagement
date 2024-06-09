using assetmgmt.core.Models.Requests;
using FluentValidation;

namespace assetmgmt.core.Validators
{
    public class CreateSourceValidator : AbstractValidator<CreateSourceRequest>
    {
        public CreateSourceValidator()
        {
            RuleFor(x => x.Name)
               .NotNull()
               .NotEmpty()
               .Length(5, 40);

            RuleFor(x => x.Price)
               .GreaterThan(0)
               .NotNull()
               .NotEmpty();
        }
    }
}
