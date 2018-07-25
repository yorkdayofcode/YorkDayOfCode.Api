using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Xunit;
using YorkDayOfCode.Api;

namespace YorkDayOfCode.API.Tests
{
    public class TokenValidatorTests
    {
        [Fact]
        public void ShouldBeValid()
        {
            Environment.SetEnvironmentVariable("SymmetricSigningKey", "fEI3U/C9kcczNXUNXxWQQFNe0GoUa8LEQbgV8/125vTFbzyo0CgNwc/vx3lfX8xHx5JwrPvUayvi9QoawRHWBA==", EnvironmentVariableTarget.Process);
            var token =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI5NjJmNzgzYy0zZjliLTQxZGUtYmM2Zi1hZWU2MDRkMWEyOTMiLCJuYmYiOjE1MzE5MDMxOTksImV4cCI6MTUzMTk4OTU5OSwiaWF0IjoxNTMxOTAzMTk5LCJpc3MiOiJodHRwczovL3lvcmtkYXlvZmNvZGUuZ2l0aHViLmlvIiwiYXVkIjoiaHR0cHM6Ly95b3JrZGF5b2Zjb2RlLmdpdGh1Yi5pbyJ9.yd9rlWxzGkZsiOSeDLUc5-8a3M1STB7qy7jGcnivpSE";

            var tokenValidator = new TokenValidator(new SymmetricSigningKey(), new JwtSecurityTokenHandler());

            var isValid = tokenValidator.IsValid(token);

            Assert.True(isValid);
        }
    }
}
