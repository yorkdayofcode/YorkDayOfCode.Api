using System;
using System.IO;
using System.Reflection;

namespace YorkDayOfCode.Api.SuggestCanvasID
{
    public class RandomWordSuggester : ICanvasIDSuggester
    {
        readonly IRandom _random;
        public RandomWordSuggester(IRandom random)
        {
            _random = random;
        }
        public string Suggest()
        {
            var words = ListOfWords();

            var word1 = words[_random.Next(words.Length) - 1];
            var word2 = words[_random.Next(words.Length) - 1];
            var word3 = words[_random.Next(words.Length) - 1];

            return $"{word1}.{word2}.{word3}";
        }

        private static string[] ListOfWords()
        {
            var assembly = typeof(RandomWordSuggester).GetTypeInfo().Assembly;
            using (var resource = assembly.GetManifestResourceStream("YorkDayOfCode.Api.SuggestCanvasID.Words.txt"))
            using (var reader = new StreamReader(resource))
            {
                return reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
