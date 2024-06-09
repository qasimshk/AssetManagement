using assetmgmt.core.Common.Results;
using assetmgmt.core.Models.Requests;
using assetmgmt.core.Models.Responses;

namespace assetmgmt.core.Abstractions.Services
{
    public interface IAssetServices
    {
        Task<Result<AssetResponse>> CreateAsset(CreateAssetRequest create);

        Task<Result<AssetResponse>> CreateSource(CreateSourceRequest create, int assetId);

        Task<Result> UpdateAsset(UpdateAssetRequest update, int assetId);

        Task<Result> SourcePriceUpdate(int sourceId, UpdatePriceRequest price);

        Task<Result<AssetResponse>> GetAssetByAssetId(int assetId);

        Task<Result<List<AssetResponse>>> SearchAssets(SearchParametersRequest search);
    }
}
