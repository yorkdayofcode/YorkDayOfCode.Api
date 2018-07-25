using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace YorkDayOfCode.Api
{
    public class AccessTokenGenerator
    {
        private readonly SymmetricSigningKey _symmetricSigningKey;

        public AccessTokenGenerator(SymmetricSigningKey symmetricSigningKey)
        {
            _symmetricSigningKey = symmetricSigningKey;
        }

        public string Generate(string userId, TimeSpan expiresIn)
        {
            var key = Convert.FromBase64String(_symmetricSigningKey.Base64Value);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            });

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = "https://yorkdayofcode.github.io",
                Issuer = "https://yorkdayofcode.github.io",
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.Add(expiresIn)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

            return signedAndEncodedToken;
        }
    }
}