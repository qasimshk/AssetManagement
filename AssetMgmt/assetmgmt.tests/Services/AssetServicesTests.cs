using assetmgmt.core.Abstractions.Mappers;
using assetmgmt.core.Mappers;
using assetmgmt.core.Models.Requests;
using assetmgmt.core.Models.Responses;
using assetmgmt.core.Services;
using assetmgmt.tests.TestData;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace assetmgmt.tests.Services
{
    public class AssetServicesTests
    {
        private AssetServices _services;
        private Mock<IValidator<CreateAssetRequest>> _mockCreateAssetRequest;
        private Mock<IValidator<UpdateAssetRequest>> _mockUpdateAssetRequest;
        private Mock<IValidator<UpdatePriceRequest>> _mockUpdatePriceRequest;
        private IAssetMapper _assetMapper;

        public AssetServicesTests()
        {
            var context = DatabaseSeeding.GetInMemoryDatabase();
            _mockCreateAssetRequest = new Mock<IValidator<CreateAssetRequest>>();
            _mockUpdateAssetRequest = new Mock<IValidator<UpdateAssetRequest>>();
            _mockUpdatePriceRequest = new Mock<IValidator<UpdatePriceRequest>>();
            _assetMapper = new AssetMapper();

            var validationFailure = new ValidationFailure("", "something went wrong");

            _mockCreateAssetRequest.Setup(x => x.ValidateAsync(It.IsAny<CreateAssetRequest>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new ValidationResult());

            _mockUpdateAssetRequest.Setup(x => x.ValidateAsync(It.IsAny<UpdateAssetRequest>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new ValidationResult());

            _mockUpdatePriceRequest.Setup(x => x.ValidateAsync(It.IsAny<UpdatePriceRequest>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new ValidationResult());

            _services = new AssetServices(context,
                _mockCreateAssetRequest.Object,
                _mockUpdateAssetRequest.Object,
                _mockUpdatePriceRequest.Object,
                _assetMapper);
        }

        [Fact]
        public async Task GetAssetByAssetId_ReturnTrue_WhenValidAssetIdProvided()
        { 
            // Act
            var result = await _services.GetAssetByAssetId(1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<AssetResponse>();
        }

        [Fact]
        public async Task GetAssetByAssetId_ReturnFalse_WhenInValidAssetIdProvided()
        {
            // Act
            var result = await _services.GetAssetByAssetId(20);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task CreateAsset_ReturnTrue_WhenValidRequestDataProvided()
        {
            // Arrange
            var request = new CreateAssetRequest("IBM Corp", "IBMC", "112233445566");

            // Act
            var result = await _services.CreateAsset(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<AssetResponse>();
        }

        [Fact]
        public async Task CreateSource_ReturnTrue_WhenValidRequestDataProvided()
        {
            // Arrange
            var request = new CreateSourceRequest("Yahoo Corp", 333.99m);
            var AssetId = 1;

            // Act
            var result = await _services.CreateSource(request, AssetId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<AssetResponse>();
        }

        [Fact]
        public async Task CreateSource_ReturnFalse_WhenDuplicateRecordProvided()
        {
            // Arrange
            var request = new CreateSourceRequest("NASDAQ", 333.99m);
            var AssetId = 1;

            // Act
            var result = await _services.CreateSource(request, AssetId);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsset_ReturnTrue_WhenValidNameRequestIsProvided()
        {
            // Arrange
            var request = new UpdateAssetRequest("Microsoft Corp", null, null);
            var AssetId = 1;

            // Act
            var result = await _services.UpdateAsset(request, AssetId);

            // Assert
            result.IsSuccess.Should().BeTrue();            
        }

        [Fact]
        public async Task UpdateAsset_ReturnFalse_WhenInValidAssetIdIsProvided()
        {
            // Arrange
            var request = new UpdateAssetRequest("Microsoft", null, null);
            var AssetId = 45;

            // Act
            var result = await _services.UpdateAsset(request, AssetId);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task SourcePriceUpdate_ReturnTrue_WhenValidRequestIsProvided()
        {
            // Arrange
            var request = new UpdatePriceRequest(34.35m);
            var SourceId = 2;

            // Act
            var result = await _services.SourcePriceUpdate(SourceId, request);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task SourcePriceUpdate_ReturnFalse_WhenSourceIdRequestIsProvided()
        {
            // Arrange
            var request = new UpdatePriceRequest(34.35m);
            var SourceId = 82;

            // Act
            var result = await _services.SourcePriceUpdate(SourceId, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}
