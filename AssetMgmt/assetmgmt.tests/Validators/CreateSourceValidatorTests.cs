using assetmgmt.core.Models.Requests;
using assetmgmt.core.Validators;
using FluentAssertions;

namespace assetmgmt.tests.Validators
{
    public class CreateSourceValidatorTests
    {
        private CreateSourceValidator _validator;

        public CreateSourceValidatorTests()
        {
            _validator = new CreateSourceValidator();
        }

        [Fact]
        public async Task CreateSourceValidator_ShouldReturnErrorList_WhenRequiredFieldsAreEmpty()
        {
            // Arrange
            var createSource = new CreateSourceRequest(string.Empty, 0);

            // Act
            var result = await _validator.ValidateAsync(createSource);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should().HaveCount(4);
        }

        [Fact]
        public async Task CreateSourceValidator_ShouldReturnTrue_WhenInsertedDataIsCorrect()
        {
            // Arrange
            var createSource = new CreateSourceRequest("Reuters", 423.85m);

            // Act
            var result = await _validator.ValidateAsync(createSource);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
