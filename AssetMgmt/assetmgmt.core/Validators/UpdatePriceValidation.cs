using assetmgmt.core.Models.Requests;
using FluentValidation;

namespace assetmgmt.core.Validators
{
    public class UpdatePriceValidation : AbstractValidator<UpdatePriceRequest>
    {
        public UpdatePriceValidation()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .NotNull()
                .NotEmpty();
        }
    }
}
