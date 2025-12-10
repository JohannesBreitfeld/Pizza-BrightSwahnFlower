using Identity.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Identity.Tests.Services
{
    public class JwtTokenServiceTests
    {
        private JwtTokenService CreateService(Dictionary<string, string> jwtSettings = null!)
        {
            var configDict = new Dictionary<string, string>
            {
                {"JwtSettings:Secret", "supersecretkey1234567890mustbelongerthanthat"},
                {"JwtSettings:Issuer", "TestIssuer"},
                {"JwtSettings:Audience", "TestAudience"},
                {"JwtSettings:ExpirationInMinutes", "60"},
                {"JwtSettings:RefreshTokenExpirationInDays", "7"}
            };
            if (jwtSettings != null)
            {
                foreach (var kvp in jwtSettings)
                    configDict[kvp.Key] = kvp.Value;
            }
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(configDict!).Build();
            return new JwtTokenService(configuration);
        }

        [Fact]
        public async Task GenerateJwtTokenAsync_ReturnsToken()
        {
            var service = CreateService();
            var user = new IdentityUser { Id = "user1", UserName = "testuser", Email = "test@example.com" };
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
            userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin", "User" });

            var token = await service.GenerateJwtTokenAsync(user, userManagerMock.Object);

            Assert.False(string.IsNullOrWhiteSpace(token));
            var parts = token.Split('.');
            Assert.Equal(3, parts.Length);
            Assert.All(parts, part => Assert.False(string.IsNullOrWhiteSpace(part)));
        }

        [Fact]
        public void GenerateRefreshToken_ReturnsBase64String()
        {
            var service = CreateService();
            var token = service.GenerateRefreshToken();
            Assert.False(string.IsNullOrWhiteSpace(token));
            var bytes = Convert.FromBase64String(token);
            Assert.Equal(64, bytes.Length);
        }

        [Fact]
        public void GetTokenExpiration_ReturnsFutureDate()
        {
            var service = CreateService();
            var expiration = service.GetTokenExpiration();
            Assert.True(expiration > DateTime.UtcNow);
            Assert.True((expiration - DateTime.UtcNow).TotalMinutes <= 61);
        }

        [Fact]
        public void GetRefreshTokenExpirationDays_ReturnsConfiguredValue()
        {
            var service = CreateService();
            var days = service.GetRefreshTokenExpirationDays();
            Assert.Equal(7, days);
        }

        [Fact]
        public void GetRefreshTokenExpirationDays_ReturnsDefaultIfMissing()
        {
            var service = CreateService(new Dictionary<string, string> { { "JwtSettings:RefreshTokenExpirationInDays", null! } });
            var days = service.GetRefreshTokenExpirationDays();
            Assert.Equal(7, days);
        }
    }
}
