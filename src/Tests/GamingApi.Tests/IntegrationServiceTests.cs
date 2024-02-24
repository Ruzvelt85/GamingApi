using System.Net;
using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using GamingApi.Integration;
using GamingApi.Integration.Config;
using GamingApi.Integration.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace GamingApi.Tests
{
    public class IntegrationServiceTests : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<HttpClientHandler> _httpClientHandlerMock;
        private bool _disposedValue;
        private readonly IOptions<IntegrationServiceSettings> _settings;
        private readonly Mock<ILogger<IntegrationService>> _loggerMock;

        public IntegrationServiceTests()
        {
            this._httpClientHandlerMock = new Mock<HttpClientHandler>();
            this._httpClient = new HttpClient(this._httpClientHandlerMock.Object, false);
            this._loggerMock = new Mock<ILogger<IntegrationService>>();
            this._settings = Options.Create(new IntegrationServiceSettings { Url = "http://localhost" });
        }
        
        [Fact]
        public void Constructor_WithNullSettings_ThrowsArgumentNullException()
        {
            var service = () => new IntegrationService(
                default!,
                this._httpClient,
                this._loggerMock.Object);
            service.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
        {
            var service = () => new IntegrationService(
                this._settings,
                default!,
                this._loggerMock.Object);
            service.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            var service = () => new IntegrationService(
                this._settings,
                this._httpClient,
                default!);
            service.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task GetGamesList_ValidRequest_ReturnsGamesListWithCorrectLimit()
        {
            // Arrange
            var request = new IntegrationServiceRequestDto { Limit = 1, Offset = 0 };
            var expectedServiceResponse = new Fixture().CreateMany<IntegrationServiceItemResponseDto>().ToArray();

            _httpClientHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedServiceResponse)),
                });

            // Act
            var results = await this.GetTarget().GetGamesAsync(request);

            // Assert
            results.Should().NotBeNull();
            results.Items.Should().NotBeNull();
            results.Items.Count.Should().Be(request.Limit);
            results.Items.Should().BeEquivalentTo(expectedServiceResponse.OrderByDescending(x => x.ReleaseDate).Skip(request.Offset).Take(request.Limit));
            results.TotalItems.Should().Be(expectedServiceResponse.Length);
            this._httpClientHandlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());
            this._httpClientHandlerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetGamesList_IncorrectConfig_ReturnsEmptyDto()
        {
            // Arrange
            var invalidSettings = Options.Create(new IntegrationServiceSettings { Url = string.Empty });
            var service = new IntegrationService(
                invalidSettings,
                this._httpClient,
                this._loggerMock.Object);

            _httpClientHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new IntegrationServiceResponseDto())),
                });

            // Act
            var results = await service.GetGamesAsync(new IntegrationServiceRequestDto());

            // Assert
            results.Should().NotBeNull();
            results.Items.Should().NotBeNull();
            results.Items.Count.Should().Be(0);
            results.TotalItems.Should().Be(0);
            this._httpClientHandlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Never(),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetGamesList_ValidRequest_ThrowsException()
        {
            // Arrange
            _httpClientHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Throws<HttpRequestException>();

            // Act
            var action = async () => await this.GetTarget().GetGamesAsync(new IntegrationServiceRequestDto());

            await action.Should().ThrowAsync<HttpRequestException>();
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    this._httpClient.Dispose();
                }

                this._disposedValue = true;
            }
        }

        private IIntegrationService GetTarget() =>
            new IntegrationService(
                this._settings,
                this._httpClient,
                this._loggerMock.Object);
    }
}
