using AutoMapper;
using GamingApi.Dto;
using GamingApi.Patterns;
using Microsoft.AspNetCore.Mvc;
using Yld.GamingApi.WebApi.Filters;
using Yld.GamingApi.WebApi.Queries;

namespace Yld.GamingApi.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public sealed class GamesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IQueryHandler<GetGameListQuery, GameListResponseDto> _getGameListQueryHandler;

    public GamesController(IMapper mapper, IQueryHandler<GetGameListQuery, GameListResponseDto> getGameListQueryHandler)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _getGameListQueryHandler = getGameListQueryHandler ?? throw new ArgumentNullException(nameof(getGameListQueryHandler));
    }

    [HttpGet]
    [ValidateUserAgentActionFilter]
    public async Task<ActionResult<GameListResponseDto>> GetGameListAsync([FromQuery] GameListRequestDto request)
    {
        var query = _mapper.Map<GetGameListQuery>(request);
        var games = await _getGameListQueryHandler.HandleAsync(query);
        return Ok(games);
    }
}
