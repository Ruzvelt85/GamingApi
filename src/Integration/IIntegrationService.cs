using GamingApi.Integration.Dto;

namespace GamingApi.Integration
{
    public interface IIntegrationService
    {
        Task<IntegrationServiceResponseDto> GetGamesAsync(IntegrationServiceRequestDto request);
    }
}
