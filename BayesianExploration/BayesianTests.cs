using System;
using NUnit.Framework;

namespace BayesianExploration
{
    [TestFixture]
    public class BayesianTests
    {
        [Test]
        public void Given_a_corpus_what_are_the_odds_of_a_certain_word_being_chosen_randomly_from_within_it()
        {
            var corpus = Corpus.OfText("A brown brown fox is more brown than a brown fox.");
            var bayes = new Bayes();
            var belief = bayes.BeliefOf("brown", corpus);

            Assert.That(belief, Is.EqualTo(4/11d), "probability should match");
        }
    }

    public class Bayes
    {
        public double BeliefOf(string word, Corpus corpus)
        {
            var occurencesOfWordInCorpus = corpus.CountOccurencesOf(word);
            var allOccurencesOfAllWords = corpus.CountAllOccurences();
            return occurencesOfWordInCorpus / (double)allOccurencesOfAllWords;
        }
    }
}