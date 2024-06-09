using assetmgmt.core.Models.Requests;
using assetmgmt.core.Validators;
using FluentAssertions;

namespace assetmgmt.tests.Validators
{
    public class UpdatePriceValidationTests
    {
        private UpdatePriceValidation _validator;

        public UpdatePriceValidationTests()
        {
            _validator = new UpdatePriceValidation();
        }

        [Fact]
        public async Task UpdatePriceValidation_ShouldReturnErrorList_WhenPriceIsZero()
        {
            // Arrange
            var updatePrice = new UpdatePriceRequest(0);

            // Act
            var result = await _validator.ValidateAsync(updatePrice);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should().HaveCount(2);
        }

        [Fact]
        public async Task UpdatePriceValidation_ShouldReturnTrue_WhenPriceIsCorrect()
        {
            // Arrange
            var updatePrice = new UpdatePriceRequest(423.56m);

            // Act
            var result = await _validator.ValidateAsync(updatePrice);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
