using assetmgmt.core.Models.Requests;
using assetmgmt.data.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace assetmgmt.core.Validators
{
    public class CreateAssetRequestValidator : AbstractValidator<CreateAssetRequest>
    {
        private readonly AssetDbContext _assetDbContext;

        public CreateAssetRequestValidator(AssetDbContext assetDbContext)
        {
            _assetDbContext = assetDbContext;

            RuleFor(x => x.Name)
                .Cascade(cascadeMode: CascadeMode.Stop)
                .MustAsync(async (name, cancellationToken) => !(await CheckAssetName(name)))
                .WithMessage(x => $"Asset with this name '{x.Name}' already exist")
                .NotNull()
                .NotEmpty()
                .Length(6, 40);

            RuleFor(x => x.Symbol)
                .Cascade(cascadeMode: CascadeMode.Stop)
                .MustAsync(async (symbol, cancellationToken) => !(await CheckAssetSymbol(symbol)))
                .WithMessage(x => $"Asset with this symbol '{x.Symbol}' already exist")
                .NotNull()
                .NotEmpty()
                .Length(4, 10);

            RuleFor(x => x.ISIN)
                .Cascade(cascadeMode: CascadeMode.Stop)
                .MustAsync(async (isin, cancellationToken) => !(await CheckAssetISIN(isin)))
                .WithMessage(x => $"Asset with this ISIN '{x.ISIN}' already exist")
                .NotNull()
                .NotEmpty()
                .Length(10, 20);
        }

        private async Task<bool> CheckAssetName(string name)
        {
            var record = _assetDbContext.Assets.Where(x => x.Name == name);
            return await record.AnyAsync();
        }

        private async Task<bool> CheckAssetSymbol(string symbol)
        {
            var record = _assetDbContext.Assets.Where(x => x.Symbol == symbol);
            return await record.AnyAsync();            
        }

        private async Task<bool> CheckAssetISIN(string isin)
        {
            var record = _assetDbContext.Assets.Where(x => x.ISIN == isin);
            return await record.AnyAsync();
        }
    }
}
