using System;
using System.Collections.Generic;
using System.Linq;

namespace BayesianExploration
{
    public class Corpus
    {
        public static Corpus OfText(string text)
        {
            return new Corpus(text);
        }

        private readonly Dictionary<string, int> Words = new Dictionary<string, int>();

        public Corpus(string value)
        {
            foreach (var word in ParseTextToWords(value))
            {
                if (Words.ContainsKey(word))
                    Words[word]++;
                else
                    Words.Add(word, 1);
            }
        }

        public Corpus()
        {
            
        }

        public int Count
        {
            get
            {
                return Words.Count;
            }
        }

        public bool Contains(string word)
        {
            return Words.ContainsKey(word);
        }

        private static IEnumerable<string> ParseTextToWords(string text)
        {
            var words = new List<string>();

            var currentWord = String.Empty;
            foreach (var character in text)
            {
                if (character == ' ' || character == '.' || character == '?' || character == ',')
                {
                    if (!String.IsNullOrWhiteSpace(currentWord))
                        words.Add(currentWord.ToLowerInvariant());

                    currentWord = String.Empty;
                }
                else
                {
                    currentWord += character;
                }
            }
            if (currentWord != String.Empty)
                words.Add(currentWord);

            return words;
        }

        public void Add(Corpus corpus)
        {
            foreach (var pair in corpus.Words)
            {
                var word = pair.Key;
                var count = pair.Value;
                if (Words.ContainsKey(word))
                    Words[word]++;
                else
                    Words.Add(word, count);
            }
        }

        public bool Contains(Corpus corpus)
        {
            var result = true;
            foreach (var word in corpus.Words)
                result &= Words.ContainsKey(word.Key);

            return result;
        }

        public int CountOccurencesOf(string word)
        {
            return Words.ContainsKey(word) ? Words[word] : 0;
        }

        public int CountAllOccurences()
        {
            return Words.Sum(x => x.Value);
        }

        public void ForEveryOccurenceOfEachWord(Action<string> action)
        {
            foreach (var word in Words)
                foreach(var index in Enumerable.Range(0, word.Value))
                    action(word.Key);
        }
    }
}