using System;
using NUnit.Framework;

namespace BayesianExploration
{
    [TestFixture]
    public class SpamDetectorTests
    {
        [Test]
        public void Given_never_before_seen_text()
        {
            var detector = new SpamDetector();
            var corpus = Corpus.OfText("some text");

            Assert.That(!detector.IsSpam(corpus), "should be ham.");
        }

        [Test]
        public void Given_text_previously_marked_as_spam()
        {
            var detector = new SpamDetector();
            var corpus = Corpus.OfText("some text");

            detector.SpamFound(corpus);

            var isSpam = detector.IsSpam(corpus);

            Assert.That(isSpam, "should be spam");
        }

        [Test]
        public void Given_text_marked_as_spam_then_marked_as_ham()
        {
            var detector = new SpamDetector();
            var spamCorpus = Corpus.OfText("this is totally NOT spam");
            detector.SpamFound(spamCorpus);
            detector.HamFound(spamCorpus);

            Assert.That(!detector.IsSpam(spamCorpus), "should not be spam");
        }

        [Test]
        public void Given_text_that_has_known_ham_and_unknown_words()
        {
            var detector = new SpamDetector();
            var ham = Corpus.OfText("this is totally ham");
            var mysteryMeat = Corpus.OfText("this is all wtf");

            detector.HamFound(ham);

            var result = detector.IsSpam(mysteryMeat);

            Assert.That(!result, "should be ham");
        }

        [Test]
        public void Given_text_that_has_an_equivalent_number_of_ham_and_spam()
        {
            var detector = new SpamDetector();
            var ham = Corpus.OfText("a b");
            var spam = Corpus.OfText("c d");
            
            detector.HamFound(ham);
            detector.SpamFound(spam);

            var result = detector.IsSpam(Corpus.OfText("a b c d"));

            Assert.That(!result, "should not be spam");
        }

        [Test]
        public void Given_a_corpus_that_is_heavily_weighted_towards_being_spam()
        {
            var ham = Corpus.OfText("a b");
            var spam = Corpus.OfText("a b c d");

            var detector = new SpamDetector();
            detector.HamFound(ham);
            detector.SpamFound(spam);
            
            var result = detector.IsSpam(Corpus.OfText("a b c d"));

            Assert.That(result, "should be spam.");
        }

        [Test]
        public void Given_a_corpus_that_is_half_spam_and_half_unknown()
        {
            var detector = new SpamDetector();
            detector.SpamFound(Corpus.OfText("a b"));

            var result = detector.IsSpam(Corpus.OfText("a b c d"));

            Assert.That(result, "should be whatever");
        }
    }
}