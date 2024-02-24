using AutoMapper;
using GamingApi.Integration;
using Moq;
using FluentAssertions;
using GamingApi.Integration.Dto;
using Yld.GamingApi.WebApi.Mapping;
using Yld.GamingApi.WebApi.Queries;
using AutoFixture;

namespace GamingApi.Tests
{
    public class QueryHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IIntegrationService> _integrationServiceMock;

        public QueryHandlerTests()
        {
            this._mapperMock = new Mock<IMapper>();
            this._integrationServiceMock = new Mock<IIntegrationService>();
        }

        [Fact]
        public void Constructor_WithNullMapper_ThrowsArgumentNullException()
        {
            var action = () => new GetGameListQueryHandler(
                default!,
                this._integrationServiceMock.Object);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullIntegrationService_ThrowsArgumentNullException()
        {
            var action = () => new GetGameListQueryHandler(
                this._mapperMock.Object,
                default!);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task GetGameList_ValidRequest_ValidResponse()
        {
            //Arrange
            const int limit = 1;
            const int total = 1;
            var expectedReturnedItems = new Fixture().CreateMany<IntegrationServiceItemResponseDto>().ToArray();
            var expectedResponse = new IntegrationServiceResponseDto
            {
                Items = expectedReturnedItems.Take(limit).ToArray(), TotalItems = total
            };
            this._integrationServiceMock
                .Setup(m => m.GetGamesAsync(It.IsAny<IntegrationServiceRequestDto>()))
                .ReturnsAsync(expectedResponse);
            var query = new GetGameListQuery(0, limit);

            //Act
            var response = await GetTarget().HandleAsync(query);

            //Assert
            response.Should().NotBeNull();
            response.TotalItems.Should().Be(total);
            response.Items.Should().HaveCount(limit);
            response.Items.First().Id.Should().Be(expectedReturnedItems.First().Appid);
        }

        private GetGameListQueryHandler GetTarget()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddMaps(typeof(GameListProfile).Assembly))
                .CreateMapper();

            return new GetGameListQueryHandler(
                mapper,
                this._integrationServiceMock.Object);
        }
    }
}
