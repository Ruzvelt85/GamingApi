using AutoMapper;
using GamingApi.Dto;
using GamingApi.Integration.Dto;
using Yld.GamingApi.WebApi.Queries;

namespace Yld.GamingApi.WebApi.Mapping
{
    public class GameListProfile : Profile
    {
        public GameListProfile()
        {
            CreateMap<GameListRequestDto, GetGameListQuery>();
            CreateMap<GetGameListQuery, IntegrationServiceRequestDto>();

            CreateMap<IntegrationServiceItemResponseDto, GameResponseDto>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Appid));

            CreateMap<IntegrationServiceResponseDto, GameListResponseDto>();
        }
    }
}
