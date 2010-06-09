using System;
using System.Diagnostics;

namespace BayesianExploration
{
    public class SpamDetector
    {
        private Corpus SpamCorpus = new Corpus();
        private Corpus HamCorpus = new Corpus();
        private Corpus CompleteCorpus = new Corpus();

        public bool IsSpam(Corpus corpus)
        {
            var countAllOccurences = (double) CompleteCorpus.CountAllOccurences();
            var beliefInCorpusBeingSpam = SpamCorpus.CountAllOccurences()/countAllOccurences;

            corpus.ForEveryOccurenceOfEachWord(
                word =>
                    {
                        var occurencesOfWord = SpamCorpus.CountOccurencesOf(word);
                        if(occurencesOfWord == 0)
                        {
                            beliefInCorpusBeingSpam = beliefInCorpusBeingSpam * .4;
                        }
                        else
                        {
                            var beliefInWordOccuringAtAll = CompleteCorpus.CountOccurencesOf(word)/countAllOccurences;
                            var beliefInEvidenceAndSpam = SpamCorpus.CountOccurencesOf(word)/countAllOccurences;
                            
                            beliefInCorpusBeingSpam = beliefInCorpusBeingSpam * beliefInEvidenceAndSpam / beliefInWordOccuringAtAll;
                        }
                        
                    });

            Debug.WriteLine(String.Format("Belief: {0}", beliefInCorpusBeingSpam));
            return beliefInCorpusBeingSpam > .16;
        }

        public void SpamFound(Corpus corpus)
        {
            SpamCorpus.Add(corpus);
            CompleteCorpus.Add(corpus);
        }

        public void HamFound(Corpus corpus)
        {
            HamCorpus.Add(corpus);
            CompleteCorpus.Add(corpus);
        }
    }
}