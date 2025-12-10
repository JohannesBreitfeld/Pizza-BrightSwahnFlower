using FluentValidation.TestHelper;
using Identity.Core.Features.Login;

namespace Identity.Tests.Features.Login
{
    public class LoginCommandValidatorTests
    {
        private readonly LoginCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Username_Is_Empty()
        {
            var command = new LoginCommand { Username = "", Password = "validPassword" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Empty()
        {
            var command = new LoginCommand { Username = "validUser", Password = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Username_And_Password_Are_Provided()
        {
            var command = new LoginCommand { Username = "validUser", Password = "validPassword" };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Username);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }
    }
}
