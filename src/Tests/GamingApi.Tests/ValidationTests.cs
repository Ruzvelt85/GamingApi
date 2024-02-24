using FluentValidation.TestHelper;
using GamingApi.Dto;
using Yld.GamingApi.WebApi.Validators;

namespace GamingApi.Tests
{
    public class ValidationTests
    {
        private readonly GameListRequestDto _defaultModel;
        private readonly GameListRequestDtoValidator _dtoValidator;

        public ValidationTests()
        {
            _defaultModel = new GameListRequestDto();
            _dtoValidator = new GameListRequestDtoValidator();
        }

        [Fact]
        public async Task Default_ShouldNotHaveValidationError()
        {
            var result = await _dtoValidator.TestValidateAsync(_defaultModel);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task CorrectValues_ShouldNotHaveValidationError()
        {
            var model = new GameListRequestDto(Offset: 2, Limit: 10);
            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task OffsetHasBigValue_ShouldNotHaveValidationError()
        {
            var model = _defaultModel with { Offset = 123000 };
            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task OffsetNegative_ShouldHaveValidationError()
        {
            var model = _defaultModel with { Offset = -2 };
            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Offset);
        }

        [Fact]
        public async Task LimitNegative_ShouldHaveValidationError()
        {
            var model = _defaultModel with { Limit = -5 };
            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Limit);
        }

        [Fact]
        public async Task LimitLargerThan10_ShouldHaveValidationError()
        {
            var model = _defaultModel with { Limit = 11 };
            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Limit);
        }
    }
}
