using Identity.Core.Contracts;
using Identity.Core.Domain;
using Identity.Core.Exceptions;
using Identity.Core.Features.Refresh;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Identity.Tests.Features.Refresh
{
    public class RefreshTokenHandlerTests
    {
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMock = new();
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock = new();
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<ILogger<RefreshTokenHandler>> _loggerMock = new();

        public RefreshTokenHandlerTests()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        }

        [Fact]
        public async Task Handle_ValidTokenAndUser_ReturnsRefreshResponse()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1" };
            var refreshToken = new RefreshToken { Token = "valid", UserId = "user1", ExpiresAt = DateTime.UtcNow.AddMinutes(10) };
            var jwt = "jwt-token";
            var expiresAt = DateTime.UtcNow.AddMinutes(15);

            _refreshTokenRepositoryMock.Setup(r => r.GetByTokenAsync("valid")).ReturnsAsync(refreshToken);
            _userManagerMock.Setup(u => u.FindByIdAsync("user1")).ReturnsAsync(user);
            _jwtTokenServiceMock.Setup(j => j.GenerateJwtTokenAsync(user, _userManagerMock.Object)).ReturnsAsync(jwt);
            _jwtTokenServiceMock.Setup(j => j.GetTokenExpiration()).Returns(expiresAt);

            var handler = new RefreshTokenHandler(
                _jwtTokenServiceMock.Object,
                _refreshTokenRepositoryMock.Object,
                _userManagerMock.Object,
                _loggerMock.Object);

            var command = new RefreshCommand("valid");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(jwt, result.Token);
            Assert.Equal("valid", result.RefreshToken);
            Assert.Equal(expiresAt, result.ExpiresAt);
        }

        [Fact]
        public async Task Handle_ExpiredOrInvalidToken_ThrowsInvalidRefreshTokenException()
        {
            // Arrange
            _refreshTokenRepositoryMock.Setup(r => r.GetByTokenAsync("expired")).ReturnsAsync((RefreshToken)null!);

            var handler = new RefreshTokenHandler(
                _jwtTokenServiceMock.Object,
                _refreshTokenRepositoryMock.Object,
                _userManagerMock.Object,
                _loggerMock.Object);

            var command = new RefreshCommand("expired");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidRefreshTokenException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_RefreshTokenForNonExistentUser_ThrowsInvalidRefreshTokenException()
        {
            // Arrange
            var refreshToken = new RefreshToken { Token = "valid", UserId = "user2", ExpiresAt = DateTime.UtcNow.AddMinutes(10) };
            _refreshTokenRepositoryMock.Setup(r => r.GetByTokenAsync("valid")).ReturnsAsync(refreshToken);
            _userManagerMock.Setup(u => u.FindByIdAsync("user2")).ReturnsAsync((IdentityUser)null!);

            var handler = new RefreshTokenHandler(
                _jwtTokenServiceMock.Object,
                _refreshTokenRepositoryMock.Object,
                _userManagerMock.Object,
                _loggerMock.Object);

            var command = new RefreshCommand("valid");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidRefreshTokenException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
