using assetmgmt.core.Abstractions.Mappers;
using assetmgmt.core.Models.Responses;
using assetmgmt.data.Entities;

namespace assetmgmt.core.Mappers
{
    public class AssetMapper : IAssetMapper
    {
        public AssetResponse Map(Asset from)
        {
            return new AssetResponse(from.Id,
                from.Name, 
                from.Symbol, 
                from.ISIN, 
                from.ModifiedOn.ToShortDateString(), 
                from.ModifiedOn.ToShortTimeString(), 
                from.Sources.Select(x => new SourceResponse(
                    x.Name, 
                    string.Format("{0} USD", x.Price.ToString("#.##")), 
                    x.ModifiedOn.ToShortDateString(), 
                    x.ModifiedOn.ToShortTimeString()))
                .OrderByDescending(x => x.Price)
                .ToList());
        }
    }
}
