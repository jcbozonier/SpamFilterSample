using NUnit.Framework;
using System.Collections.Generic;

namespace BayesianExploration
{
    [TestFixture]
    public class CorpusTests
    {
        [Test]
        public void SimpleCase()
        {
            var corpus = Corpus.OfText("Hello this is a corpus.");

            Assert.That(corpus.Count, Is.EqualTo(5));
            Assert.That(corpus.Contains("hello"), "It should store the lowercase version of the words.");
            Assert.That(corpus.Contains("corpus"), "It should remove the punctuation marks.");
        }

        [Test]
        public void MoreComplex()
        {
            var corpus = Corpus.OfText("Hello this is a corpus, my corpus. Do you have corpus's?");

            Assert.That(corpus.Count, Is.EqualTo(10), "words should not be added twice.");
            Assert.That(corpus.Contains("corpus's"), "should contain a word with punctuation but not word separators.");
            Assert.That(corpus.Contains("corpus"), "should not count a comma as a part of a word");
            Assert.That(corpus.CountOccurencesOf("corpus"), Is.EqualTo(2), "should track occurences of words in itself");

            Assert.That(corpus.CountAllOccurences(), Is.EqualTo(11), "should track total number of non-distinct words.");
        }

        [Test]
        public void CombiningCorpuses()
        {
            var corpus1 = Corpus.OfText("a b c");
            var corpus2 = Corpus.OfText("abc");

            corpus1.Add(corpus2);

            Assert.That(corpus1.Contains("abc"), "should be combinable.");
        }

        [Test]
        public void CorpusContainsOtherCorpus()
        {
            var corpus1 = Corpus.OfText("a b c d");
            var corpus2 = Corpus.OfText("b d");

            Assert.That(corpus1.Contains(corpus2), "should contain the other corpus");
        }

        [Test]
        public void CorpusIteratesThroughEveryOccurenceOfEveryWord()
        {
            var words = new List<string>();
            var text = "a a b";
            var corpus1 = Corpus.OfText(text);
            corpus1.ForEveryOccurenceOfEachWord(words.Add);

            Assert.That(words, Is.EquivalentTo(text.Split(' ')), "every word from the corpus should be represented in the iteration.");
        }
    }
}
