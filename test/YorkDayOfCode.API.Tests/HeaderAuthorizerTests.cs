using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using YorkDayOfCode.Api;

namespace YorkDayOfCode.API.Tests
{
    public class HeaderAuthorizerTests
    {
        private readonly HeaderAuthorizer _headerAuthorizer;
        private readonly Mock<ITokenValidator> _tokenValidator;

        public HeaderAuthorizerTests()
        {
            _tokenValidator = new Mock<ITokenValidator>();
            _headerAuthorizer = new HeaderAuthorizer(_tokenValidator.Object);
        }

        [Fact]
        public void ShouldReturnFalseWhenNoAuthorizationExists()
        {
            var result = _headerAuthorizer.Authorize(new Dictionary<string, string>());

            Assert.False(result);
        }

        [Fact]
        public void ShouldReturnFalseWhenNotBearerToken()
        {
            var result = _headerAuthorizer.Authorize(new Dictionary<string, string>()
            {
                {"Authorization", "Basic 123"}
            });

            Assert.False(result);
        }

        [Fact]
        public void ShouldReturnFalseWhenBearerTokenIsIncorrect()
        {
            var token = Guid.NewGuid().ToString();
            _tokenValidator.Setup(x => x.IsValid(token))
                .Returns(false);

            var result = _headerAuthorizer.Authorize(new Dictionary<string, string>()
            {
                {"Authorization", $"Bearer {token}"}
            });

            Assert.False(result);
        }

        [Fact]
        public void ShouldReturnFalseWhenThereIsNoToken()
        {
            var result = _headerAuthorizer.Authorize(new Dictionary<string, string>()
            {
                {"Authorization", $"Bearer"}
            });

            Assert.False(result);
        }

        [Fact]
        public void ShouldReturnTrueWhenTokenIsValid()
        {
            var token = Guid.NewGuid().ToString();
            _tokenValidator.Setup(x => x.IsValid(token))
                .Returns(true);

            var result = _headerAuthorizer.Authorize(new Dictionary<string, string>()
            {
                {"Authorization", $"Bearer {token}"}
            });

            Assert.True(result);
        }
    }
}
