using System.Text.Json.Serialization;

namespace GamingApi.Integration.Dto
{
    public record IntegrationServiceItemResponseDto
    {
        public int Appid { get; init; }

        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("short_description")]
        public string ShortDescription { get; init; } = string.Empty;

        public string Developer { get; init; } = string.Empty;

        public string Publisher { get; init; } = string.Empty;

        public string Genre { get; init; } = string.Empty;

        public IDictionary<string, int> Tags { get; init; } = new Dictionary<string, int>();

        public string Type { get; init; } = string.Empty;
        
        public IReadOnlyCollection<string> Categories { get; set; } = Array.Empty<string>();

        public string Owners { get; init; } = string.Empty;

        public int Positive { get; init; }

        public int Negative { get; init; }

        public string Price { get; init; } = string.Empty;

        public string InitialPrice { get; init; } = string.Empty;

        public string Discount { get; init; } = string.Empty;

        public int Ccu { get; init; }

        public string Languages { get; init; } = string.Empty;

        public IDictionary<string, bool> Platforms { get; init; } = new Dictionary<string, bool>();

        [JsonPropertyName("release_date")]
        public DateTime ReleaseDate { get; init; }

        [JsonPropertyName("required_age")]
        public int RequiredAge { get; init; }

        public string Website { get; init; } = string.Empty;

        [JsonPropertyName("header_image")]
        public string HeaderImage { get; init; } = string.Empty;
    }
}
