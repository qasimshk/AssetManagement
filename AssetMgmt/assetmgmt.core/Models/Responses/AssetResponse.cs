namespace assetmgmt.core.Models.Responses
{
    public record class AssetResponse(int AssetId,
        string Name,
        string Symbol,
        string ISIN,
        string RecordDate,
        string RecordTime,
        List<SourceResponse>? Sources);   
}
