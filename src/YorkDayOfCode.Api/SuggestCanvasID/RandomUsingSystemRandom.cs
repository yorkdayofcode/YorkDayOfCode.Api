using System;

namespace YorkDayOfCode.Api.SuggestCanvasID
{
    public class RandomUsingSystemRandom : IRandom
    {
        private readonly Random _random = new Random();
        public int Next(int numberOfWords)
        {
            return _random.Next(1, numberOfWords + 1);
        }
    }
}
