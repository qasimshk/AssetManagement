using assetmgmt.core.Abstractions.Mappers;
using assetmgmt.core.Abstractions.Services;
using assetmgmt.core.Common.Results;
using assetmgmt.core.Models.Requests;
using assetmgmt.core.Models.Responses;
using assetmgmt.data.Context;
using assetmgmt.data.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace assetmgmt.core.Services
{
    public class AssetServices(AssetDbContext assetDbContext,
        IValidator<CreateAssetRequest> createValidator,
        IValidator<UpdateAssetRequest> updateValidator,
        IValidator<UpdatePriceRequest> updatePriceValidator,
        IAssetMapper assetMapper) : IAssetServices
    {
        private readonly AssetDbContext _assetDbContext = assetDbContext;
        private readonly IValidator<CreateAssetRequest> _createValidator = createValidator;
        private readonly IValidator<UpdateAssetRequest> _updateValidator = updateValidator;
        private readonly IValidator<UpdatePriceRequest> _updatePriceValidator = updatePriceValidator;
        private readonly IAssetMapper _assetMapper = assetMapper;

        public async Task<Result<AssetResponse>> GetAssetByAssetId(int assetId)
        {
            var assets = await GetAssets().SingleOrDefaultAsync(x => x.Id == assetId);

            return assets != null ?
                Result<AssetResponse>.SuccessResult(_assetMapper.Map(assets)) :
                Result<AssetResponse>.FailedResult("Asset not found with this Id", HttpStatusCode.NotFound);
        }

        public async Task<Result<List<AssetResponse>>> SearchAssets(SearchParametersRequest search)
        {
            var raw = GetAssets();
            
            switch (string.IsNullOrEmpty(search.Source))
            {
                case false when search.Date != null:
                    raw = raw.Where(asset => asset.Sources.Any(src => 
                                    src.Name == search.Source && 
                                    src.ModifiedOn.Date == search.Date));
                    break;
                case false:
                    raw = raw.Where(asset => asset.Sources.Any(src => src.Name == search.Source));
                    break;
                default:
                    if (search.Date != null)
                    {
                        raw = raw.Where(asset => asset.Sources.Any(src => src.ModifiedOn.Date == search.Date));
                    }
                    break;
            }
            return await raw.CountAsync() > 0 ?
                Result<List<AssetResponse>>.SuccessResult(raw.Select(_assetMapper.Map).ToList()):
                Result<List<AssetResponse>>.FailedResult("Asset not found with this search", HttpStatusCode.NotFound);
        }

        public async Task<Result<AssetResponse>> CreateAsset(CreateAssetRequest create)
        {
            var result = await _createValidator.ValidateAsync(create);

            if (result.IsValid)
            {
                var asset = Asset.Create(create.Name, create.Symbol, create.ISIN);

                _assetDbContext.Assets.Add(asset);

                await _assetDbContext.SaveChangesAsync();

                return Result<AssetResponse>.SuccessResult(_assetMapper.Map(asset));
            }
            return Result<AssetResponse>.FailedResult(result.Errors.Select(x => x.ErrorMessage).First(), HttpStatusCode.BadRequest);
        }

        public async Task<Result<AssetResponse>> CreateSource(CreateSourceRequest create, int assetId)
        {
            var existingAsset = await GetAssets().SingleOrDefaultAsync(x => x.Id == assetId);
            
            if (existingAsset != null)
            {
                var checkRecord = await GetAssets().Where(x => 
                                                    x.Sources.Any(src => src.AssetId == assetId 
                                                        && src.Name == create.Name))
                                                   .ToListAsync();

                if (!checkRecord.Any())
                {
                    var source = Source.Create(create.Name, create.Price, existingAsset);

                    _assetDbContext.Sources.Add(source);

                    await _assetDbContext.SaveChangesAsync();

                    return await GetAssetByAssetId(existingAsset.Id);
                }
                return Result<AssetResponse>.FailedResult(null, HttpStatusCode.BadRequest);
            }
            return Result<AssetResponse>.FailedResult(null, HttpStatusCode.NotFound);
        }

        public async Task<Result> UpdateAsset(UpdateAssetRequest update, int assetId)
        {
            var result = await _updateValidator.ValidateAsync(update);

            if (result.IsValid)
            {
                var existingAsset = await GetAssets().SingleOrDefaultAsync(x => x.Id == assetId);
                
                if (existingAsset != null)
                {
                    existingAsset = Asset.Update(update.Name, update.Symbol, update.ISIN, existingAsset);

                    _assetDbContext.Assets.Update(existingAsset);

                    await _assetDbContext.SaveChangesAsync();

                    return Result.SuccessResult();
                }
                return Result.FailedResult(null, HttpStatusCode.NotFound);
            }
            return Result.FailedResult(result.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
        }

        public async Task<Result> SourcePriceUpdate(int sourceId, UpdatePriceRequest update)
        {
            var result = await _updatePriceValidator.ValidateAsync(update);

            if (result.IsValid)
            {
                var existingSource = await _assetDbContext.Sources.SingleOrDefaultAsync(x => x.Id == sourceId);

                if (existingSource != null)
                {
                    existingSource = Source.Update(update.Price, existingSource);

                    _assetDbContext.Sources.Update(existingSource);

                    await _assetDbContext.SaveChangesAsync();

                    return Result.SuccessResult();
                }
            }
            return Result.FailedResult(result.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
        }

        private IQueryable<Asset> GetAssets() => 
            _assetDbContext.Assets.Include(src => src.Sources);
    }
}
