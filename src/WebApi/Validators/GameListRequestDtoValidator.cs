using FluentValidation;
using GamingApi.Dto;

namespace Yld.GamingApi.WebApi.Validators
{
    public class GameListRequestDtoValidator : AbstractValidator<GameListRequestDto>
    {
        public GameListRequestDtoValidator()
        {
            RuleFor(_ => _.Offset).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(_ => _.Limit).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(10);
        }
    }
}
