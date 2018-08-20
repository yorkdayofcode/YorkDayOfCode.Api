using Xunit;
using YorkDayOfCode.Api;
using YorkDayOfCode.Api.Models.Users;
using YorkDayOfCode.Api.Validation;

namespace YorkDayOfCode.API.Tests.Validation
{
    public class UserValidatorTests
    {
        private readonly UserValidator _userValidator;

        public UserValidatorTests()
        {
            _userValidator = new UserValidator();
        }

        [Fact]
        public void ShouldBeValid()
        {
            var user = new User
            {
                Gender = Gender.Female,
                Age =  Age.ThirtyFiveToFortyFour,
                Area = Area.Bishopthorpe,
                Experience = Experience.Educational
            };

            var result = _userValidator.Validate(user);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ShouldBeInValid()
        {
            var user = new User();

            var result = _userValidator.Validate(user);

            Assert.False(result.IsValid);
        }
    }
}
