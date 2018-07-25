using System;
using Microsoft.IdentityModel.Tokens;

namespace YorkDayOfCode.Api
{
    public class TokenValidator : ITokenValidator
    {
        private readonly SymmetricSigningKey _symmetricSigningKey;
        private readonly ISecurityTokenValidator _securityTokenValidator;

        public TokenValidator(SymmetricSigningKey symmetricSigningKey, ISecurityTokenValidator securityTokenValidator)
        {
            _symmetricSigningKey = symmetricSigningKey;
            _securityTokenValidator = securityTokenValidator;
        }
        public bool IsValid(string token)
        {
            var key = Convert.FromBase64String(_symmetricSigningKey.Base64Value);

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                //ClockSkew = TimeSpan.FromHours(DateTime.Now.Hour - DateTimeOffset.UtcNow.Hour)
            };

            try
            {
                var claimsPrincipal = _securityTokenValidator.ValidateToken(token,
                    tokenValidationParameters, out var validatedToken);
                return claimsPrincipal != null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}