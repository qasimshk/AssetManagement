using assetmgmt.core.Models.Requests;
using assetmgmt.core.Validators;
using assetmgmt.data.Context;
using assetmgmt.tests.TestData;
using FluentAssertions;

namespace assetmgmt.tests.Validators
{
    public class UpdateAssetRequestValidatorTests
    {
        private AssetDbContext _assetDbContext;
        private UpdateAssetRequestValidator _validator;

        public UpdateAssetRequestValidatorTests()
        {
            _assetDbContext = DatabaseSeeding.GetInMemoryDatabase();
            _validator = new UpdateAssetRequestValidator(_assetDbContext);
        }

        [Fact]
        public async Task UpdateAssetRequestValidator_ShouldReturnErrorList_WhenExisitingDataInserted()
        {
            // Arrange
            var UpdateAsset = new UpdateAssetRequest("Microsoft Corporation", "MSFT", "US5949181045");

            // Act
            var result = await _validator.ValidateAsync(UpdateAsset);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should().HaveCount(3);
        }

        [Fact]
        public async Task UpdateAssetRequestValidator_ShouldReturnErrorList_WhenRequiredFieldsAreEmpty()
        {
            // Arrange
            var updateAsset = new UpdateAssetRequest(string.Empty, string.Empty, string.Empty);

            // Act
            var result = await _validator.ValidateAsync(updateAsset);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should().HaveCount(3);
        }

        [Fact]
        public async Task UpdateAssetRequestValidator_ShouldReturnTrue_WhenCorrectDataInserted()
        {
            // Arrange
            var updateAsset = new UpdateAssetRequest("Google Corporation", "GGLCO", "123456789AMN");

            // Act
            var result = await _validator.ValidateAsync(updateAsset);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAssetRequestValidator_ShouldReturnErrorList_WhenInsertedDataLengthIsMinimum()
        {
            // Arrange
            var updateAsset = new UpdateAssetRequest("Test1", "TST1", "TEST1");

            // Act
            var result = await _validator.ValidateAsync(updateAsset);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should().HaveCount(2);
        }
    }
}
