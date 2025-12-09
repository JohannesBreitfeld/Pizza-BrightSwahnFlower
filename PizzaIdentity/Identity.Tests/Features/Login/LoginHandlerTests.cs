using Identity.Core.Contracts;
using Identity.Core.Domain;
using Identity.Core.Exceptions;
using Identity.Core.Features.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Identity.Tests.Features.Login
{
    public class LoginHandlerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
        private readonly Mock<ILogger<LoginHandler>> _loggerMock;
        private readonly LoginHandler _handler;

        public LoginHandlerTests()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();
            _loggerMock = new Mock<ILogger<LoginHandler>>();
            _handler = new LoginHandler(
                _userManagerMock.Object,
                _refreshTokenRepositoryMock.Object,
                _jwtTokenServiceMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsLoginResponse()
        {
            // Arrange
            var user = new IdentityUser { Id = "userId", UserName = "testuser" };
            var command = new LoginCommand { Username = "testuser",Password = "password" };
            _userManagerMock.Setup(x => x.FindByNameAsync(command.Username)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password)).ReturnsAsync(true);
            _jwtTokenServiceMock.Setup(x => x.GenerateJwtTokenAsync(user, _userManagerMock.Object)).ReturnsAsync("jwtToken");
            _jwtTokenServiceMock.Setup(x => x.GetTokenExpiration()).Returns(DateTime.UtcNow.AddMinutes(15));
            _jwtTokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns("refreshToken");
            _jwtTokenServiceMock.Setup(x => x.GetRefreshTokenExpirationDays()).Returns(7);
            _refreshTokenRepositoryMock.Setup(x => x.AddAsync(It.IsAny<RefreshToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("jwtToken", result.Token);
            Assert.Equal("refreshToken", result.RefreshToken);
        }

        [Fact]
        public async Task Handle_InvalidCredentials_ThrowsInvalidCredentialsException()
        {
            // Arrange
            var command = new LoginCommand { Username = "testuser", Password = "wrongpassword" };
            _userManagerMock.Setup(x => x.FindByNameAsync(command.Username)).ReturnsAsync((IdentityUser)null!);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidCredentialsException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_RefreshTokenIsAddedToRepository()
        {
            // Arrange
            var user = new IdentityUser { Id = "userId", UserName = "testuser" };
            var command = new LoginCommand { Username = "testuser", Password = "password" };
            _userManagerMock.Setup(x => x.FindByNameAsync(command.Username)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password)).ReturnsAsync(true);
            _jwtTokenServiceMock.Setup(x => x.GenerateJwtTokenAsync(user, _userManagerMock.Object)).ReturnsAsync("jwtToken");
            _jwtTokenServiceMock.Setup(x => x.GetTokenExpiration()).Returns(DateTime.UtcNow.AddMinutes(15));
            _jwtTokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns("refreshToken");
            _jwtTokenServiceMock.Setup(x => x.GetRefreshTokenExpirationDays()).Returns(7);
            _refreshTokenRepositoryMock.Setup(x => x.AddAsync(It.IsAny<RefreshToken>())).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _refreshTokenRepositoryMock.Verify(x => x.AddAsync(It.Is<RefreshToken>(t => t.Token == "refreshToken" && t.UserId == "userId")), Times.Once);
        }
    }
}
