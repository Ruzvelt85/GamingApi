using System.Text.Json;
using GamingApi.Integration.Config;
using GamingApi.Integration.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
// ReSharper disable PossibleMultipleEnumeration

namespace GamingApi.Integration
{
    public class IntegrationService : IIntegrationService
    {
        private readonly IntegrationServiceSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public IntegrationService(IOptions<IntegrationServiceSettings> settings, HttpClient httpClient, ILogger<IntegrationService> logger)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IntegrationServiceResponseDto> GetGamesAsync(IntegrationServiceRequestDto request)
        {
            if (string.IsNullOrEmpty(_settings.Url))
            {
                _logger.LogError($"Configuration for integration service is missing");
                return new IntegrationServiceResponseDto();
            }
            try
            {
                var response = await _httpClient.GetAsync(_settings.Url);
                
                if (response.IsSuccessStatusCode)
                {
                    await using var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var receivedData = await JsonSerializer.DeserializeAsync<IEnumerable<IntegrationServiceItemResponseDto>>(responseStream, options);

                    if (receivedData != null)
                    {
                        var items = receivedData
                            .OrderByDescending(x => x.ReleaseDate)
                            .Skip(request.Offset)
                            .Take(request.Limit)
                            .ToArray();
                        var totalCount = receivedData.Count();

                        return new IntegrationServiceResponseDto
                        {
                            Items = items,
                            TotalItems = totalCount
                        };
                    }
                }

                _logger.LogError("Unknown error occurred while requesting data from integration service...");
                return new IntegrationServiceResponseDto();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while executing {nameof(GetGamesAsync)}: {ex.Message}");
                throw;
            }
        }
    }
}
