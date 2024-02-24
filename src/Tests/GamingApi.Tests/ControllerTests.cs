using AutoFixture;
using AutoMapper;
using FluentAssertions;
using GamingApi.Dto;
using GamingApi.Patterns;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Yld.GamingApi.WebApi.Controllers;
using Yld.GamingApi.WebApi.Mapping;
using Yld.GamingApi.WebApi.Queries;

namespace GamingApi.Tests
{
    public class ControllerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IQueryHandler<GetGameListQuery, GameListResponseDto>> _getGameListQueryHandlerMock;

        public ControllerTests()
        {
            this._mapperMock = new Mock<IMapper>();
            this._getGameListQueryHandlerMock = new Mock<IQueryHandler<GetGameListQuery, GameListResponseDto>>();
        }

        [Fact]
        public void Constructor_WithNullMapper_ThrowsArgumentNullException()
        {
            var controller = () => new GamesController(
                default!,
                this._getGameListQueryHandlerMock.Object);
            controller.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullQueryHandler_ThrowsArgumentNullException()
        {
            var controller = () => new GamesController(
                this._mapperMock.Object,
                default!);
            controller.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task GetGameListAsync_ValidRequestDto_ReturnsOkContent()
        {
            // Arrange
            var mapper = new MapperConfiguration(cfg => cfg.AddMaps(typeof(GameListProfile).Assembly))
                .CreateMapper();
            var targetController = new GamesController(mapper, this._getGameListQueryHandlerMock.Object);
            var expectedResponse = new GameListResponseDto
            {
                Items = new [] { new Fixture().Build<GameResponseDto>().Create() },
                TotalItems = 20
            };
            this._getGameListQueryHandlerMock
                .Setup(m => m.HandleAsync(It.IsAny<GetGameListQuery>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var actionResult = await targetController.GetGameListAsync(new GameListRequestDto());

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            Assert.IsType<GameListResponseDto>(result!.Value);
            this._getGameListQueryHandlerMock.Verify(
                queryHandler => queryHandler.HandleAsync(It.IsAny<GetGameListQuery>()),
                Times.Once);
            this._getGameListQueryHandlerMock.VerifyNoOtherCalls();
        }
    }
}
