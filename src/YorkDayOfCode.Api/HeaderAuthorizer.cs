using System;
using System.Collections.Generic;

namespace YorkDayOfCode.Api
{
    public class HeaderAuthorizer
    {
        private readonly ITokenValidator _tokenValidator;

        public HeaderAuthorizer(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }
        public bool Authorize(IDictionary<string, string> headers)
        {
            if (!headers.ContainsKey("Authorization"))
            {
                return false;
            }

            var authorizationHeader = headers["Authorization"].Split(' ');
            if (!authorizationHeader[0].Equals("Bearer", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            if (authorizationHeader.Length == 1 || !_tokenValidator.IsValid(authorizationHeader[1]))
            {
                return false;
            }

            return true;
        }
    }
}