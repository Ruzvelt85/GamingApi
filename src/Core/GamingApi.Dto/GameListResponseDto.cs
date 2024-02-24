namespace GamingApi.Dto
{
    public record GameListResponseDto
    {
        public IReadOnlyCollection<GameResponseDto> Items { get; init; } = Array.Empty<GameResponseDto>();

        public int TotalItems { get; init; } = 0;
    }
}
