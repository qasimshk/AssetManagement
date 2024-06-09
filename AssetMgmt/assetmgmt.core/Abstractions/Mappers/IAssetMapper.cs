using assetmgmt.core.Models.Responses;
using assetmgmt.data.Entities;

namespace assetmgmt.core.Abstractions.Mappers
{
    public interface IAssetMapper : 
        IMapper<Asset, AssetResponse>
    { }
}
