namespace YorkDayOfCode.Api
{
    public interface ITokenValidator
    {
        bool IsValid(string token);
    }
}