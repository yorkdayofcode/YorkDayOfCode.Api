using Xunit;
using YorkDayOfCode.Api.SuggestCanvasID;

namespace YorkDayOfCode.API.Tests
{
    public class TestRandomWordSuggester
    {
        [Fact]
        public void TheServiceShouldProduceThreeWords()
        {
            var rnd = new RandomUsingSystemRandom();
            var service = new RandomWordSuggester(rnd);
            var id = service.Suggest();
            Assert.NotEqual("", id);
            Assert.Equal(2, id.Length - id.Replace(".", "").Length);
        }

        [Fact]
        public void WithAKnownKeyTheIDShouldBeActionAgainAll()
        {
            var rnd = new MockRandom(new[] { 10, 20, 30 });
            var service = new RandomWordSuggester(rnd);
            var id = service.Suggest();
            Assert.Equal("action.again.all", id);
        }

        private class MockRandom : IRandom
        {
            readonly int[] _numbers;
            int _nextIndex;
            public MockRandom(int[] numbers)
            {
                _numbers = numbers;
            }
            public int Next(int numberOfWords)
            {
                _nextIndex++;
                return _numbers[_nextIndex - 1];
            }
        }

    }
}
