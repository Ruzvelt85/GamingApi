namespace GamingApi.Integration.Dto
{
    public record IntegrationServiceResponseDto
    {
        public IReadOnlyCollection<IntegrationServiceItemResponseDto> Items { get; init; } = Array.Empty<IntegrationServiceItemResponseDto>();

        public int TotalItems { get; init; } = 0;
    }
}
