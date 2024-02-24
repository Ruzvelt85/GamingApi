using AutoMapper;
using GamingApi.Dto;
using GamingApi.Integration;
using GamingApi.Integration.Dto;
using GamingApi.Patterns;

namespace Yld.GamingApi.WebApi.Queries
{
    public class GetGameListQueryHandler : IQueryHandler<GetGameListQuery, GameListResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IIntegrationService _integrationService;

        public GetGameListQueryHandler(IMapper mapper,
            IIntegrationService integrationService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _integrationService = integrationService ?? throw new ArgumentNullException(nameof(integrationService));
        }

        public async Task<GameListResponseDto> HandleAsync(GetGameListQuery query)
        {
            var gamesRequest = _mapper.Map<IntegrationServiceRequestDto>(query);

            var gamesResponse = await _integrationService.GetGamesAsync(gamesRequest);

            return _mapper.Map<GameListResponseDto>(gamesResponse);
        }
    }
}
