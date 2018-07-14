using System;

namespace YorkDayOfCode.Api
{
    public class SymmetricSigningKey
    {
        public string Base64Value { get; } = Environment.GetEnvironmentVariable("SymmetricSigningKey");
    }
}