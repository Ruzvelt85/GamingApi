namespace GamingApi.Dto
{
    public record GameResponseDto
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string ShortDescription { get; init; } = string.Empty;

        public string Publisher { get; init; } = string.Empty;

        public string Genre { get; init; } = string.Empty;

        public IReadOnlyCollection<string> Categories { get; init; } = Array.Empty<string>();

        public IDictionary<string, bool> Platforms { get; init; } = new Dictionary<string, bool>();

        public DateTime ReleaseDate { get; init; }

        public int RequiredAge { get; init; }
    }
}
