using assetmgmt.data.Context;
using assetmgmt.tests.TestData;
using assetmgmt.core.Validators;
using assetmgmt.core.Models.Requests;
using FluentAssertions;

namespace assetmgmt.tests.Validators
{
    public class CreateAssetValidatorTests
    {
        private AssetDbContext _assetDbContext;
        private CreateAssetRequestValidator _validator;

        public CreateAssetValidatorTests()
        {
            _assetDbContext = DatabaseSeeding.GetInMemoryDatabase();
            _validator = new CreateAssetRequestValidator(_assetDbContext);
        }

        [Fact]
        public async Task CreateAssetRequestValidator_ShouldReturnErrorList_WhenExisitingDataInserted()
        {
            // Arrange
            var createAsset = new CreateAssetRequest("Microsoft Corporation", "MSFT", "US5949181045");

            // Act
            var result = await _validator.ValidateAsync(createAsset);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should().HaveCount(3);
        }

        [Fact]
        public async Task CreateAssetRequestValidator_ShouldReturnErrorList_WhenRequiredFieldsAreEmpty()
        {
            // Arrange
            var createAsset = new CreateAssetRequest(string.Empty, string.Empty, string.Empty);

            // Act
            var result = await _validator.ValidateAsync(createAsset);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should().HaveCount(3);
        }

        [Fact]
        public async Task CreateAssetRequestValidator_ShouldReturnTrue_WhenCorrectDataInserted()
        {
            // Arrange
            var createAsset = new CreateAssetRequest("IBM Corporation", "IMBC", "123456789QASD");

            // Act
            var result = await _validator.ValidateAsync(createAsset);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAssetRequestValidator_ShouldReturnErrorList_WhenInsertedDataLengthIsMinimum()
        {
            // Arrange
            var createAsset = new CreateAssetRequest("Test", "TST", "TEST");

            // Act
            var result = await _validator.ValidateAsync(createAsset);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should().HaveCount(3);
        }
    }
}
