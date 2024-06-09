using assetmgmt.core.Abstractions.Services;
using assetmgmt.core.Common.Results;
using assetmgmt.core.Models.Requests;
using assetmgmt.core.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace assetmgmt.api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AssetController(IAssetServices assetServices) : Controller
    {
        private readonly IAssetServices _assetServices = assetServices;

        [HttpGet("{assetId}:int", Name = nameof(GetAssetByAssetId))]
        [ProducesResponseType(typeof(Result<AssetResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAssetByAssetId([FromRoute] int assetId)
        {
            var result = await _assetServices.GetAssetByAssetId(assetId);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("Search")]
        [ProducesResponseType(typeof(Result<AssetResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> SearchAsset([FromQuery] SearchParametersRequest search)
        {
            // Search date format should be year-month-day example: 2024-06-09 

            var result = await _assetServices.SearchAssets(search);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        

        [HttpPost]
        [ProducesResponseType(typeof(Result<AssetResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Result<AssetResponse>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsset([FromBody] CreateAssetRequest request)
        {
            var result = await _assetServices.CreateAsset(request);

            return result.IsSuccess ?
                CreatedAtRoute(nameof(GetAssetByAssetId), new { assetId = result.Value.AssetId }, result) :
                BadRequest(result);
        }

        [HttpPost("{assetId}:int/Source")]
        [ProducesResponseType(typeof(Result<AssetResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Result<AssetResponse>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateSource([FromRoute] int assetId, [FromBody] CreateSourceRequest create)
        {
            var result = await _assetServices.CreateSource(create, assetId);

            return result.IsSuccess ?
                CreatedAtRoute(nameof(GetAssetByAssetId), new { assetId = result.Value.AssetId }, result) :
                BadRequest(result);
        }

        [HttpPut("{assetId}:int")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCustomer([FromRoute] int assetId, [FromBody] UpdateAssetRequest update)
        {
            var result = await _assetServices.UpdateAsset(update, assetId);

            return result.StatusCode switch
            {
                HttpStatusCode.BadRequest => BadRequest(result),
                HttpStatusCode.NotFound => NotFound(),
                _ => NoContent()
            };
        }

        [HttpPut("Source/{sourceId}:int/Price")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateSourcePrice([FromRoute] int sourceId, [FromBody] UpdatePriceRequest update)
        {
            var result = await _assetServices.SourcePriceUpdate(sourceId, update);

            return result.StatusCode switch
            {
                HttpStatusCode.BadRequest => BadRequest(result),
                HttpStatusCode.NotFound => NotFound(),
                _ => NoContent()
            };
        }
    }
}
