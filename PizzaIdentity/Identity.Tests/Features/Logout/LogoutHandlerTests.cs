using Identity.Core.Contracts;
using Identity.Core.Domain;
using Identity.Core.Features.Logout;
using Microsoft.Extensions.Logging;
using Moq;
using MediatR;

namespace Identity.Tests.Features.Logout
{
    public class LogoutHandlerTests
    {
        private readonly Mock<ILogger<LogoutHandler>> _loggerMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepoMock;
        private readonly LogoutHandler _handler;

        public LogoutHandlerTests()
        {
            _loggerMock = new Mock<ILogger<LogoutHandler>>();
            _refreshTokenRepoMock = new Mock<IRefreshTokenRepository>();
            _handler = new LogoutHandler(_loggerMock.Object, _refreshTokenRepoMock.Object);
        }

        [Fact]
        public async Task Handle_RefreshTokenNotFound_LogsWarningAndReturnsUnit()
        {
            var command = new LogoutCommand { RefreshToken = "notfound" };
            _refreshTokenRepoMock.Setup(r => r.GetByTokenAsync(command.RefreshToken))
                .ReturnsAsync((RefreshToken)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Refresh token not found.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_RefreshTokenAlreadyRevoked_LogsInformationAndReturnsUnit()
        {
            var command = new LogoutCommand { RefreshToken = "revokedtoken" };
            var token = new RefreshToken { Token = command.RefreshToken, Revoked = true, UserId = "user1" };
            _refreshTokenRepoMock.Setup(r => r.GetByTokenAsync(command.RefreshToken))
                .ReturnsAsync(token);

            var result = await _handler.Handle(command, CancellationToken.None);

            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Refresh token already revoked.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_RefreshTokenValid_RevokesAndLogsInformation()
        {
            var command = new LogoutCommand { RefreshToken = "validtoken" };
            var token = new RefreshToken { Token = command.RefreshToken, Revoked = false, UserId = "user2" };
            _refreshTokenRepoMock.Setup(r => r.GetByTokenAsync(command.RefreshToken))
                .ReturnsAsync(token);
            _refreshTokenRepoMock.Setup(r => r.RevokeAsync(token.Token)).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            _refreshTokenRepoMock.Verify(r => r.RevokeAsync(token.Token), Times.Once);
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Revoked refresh token")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
            Assert.Equal(Unit.Value, result);
        }
    }
}
