using System;
using System.Collections.Generic;
using MatcherChief.Core.Models;

namespace MatcherChief.Core.Matchmaking.PreferenceScore
{
    public interface IPreferenceScoreCalculator
    {
        Dictionary<Preference, int> GetScores(IEnumerable<MatchRequest> requests);
    }

    public class PreferenceScoreCalculator : IPreferenceScoreCalculator
    {
        private const int SECONDS_PER_SCORE_WEIGHT_INCREASE = 5;

        public Dictionary<Preference, int> GetScores(IEnumerable<MatchRequest> requests)
        {
            var now = DateTime.Now;
            var scores = new Dictionary<Preference, int>();

            foreach (var request in requests)
            {
                var preferences = new List<Preference>();
                foreach (var title in request.Titles)
                    foreach (var mode in request.Modes)
                        preferences.Add(new Preference(title, mode));

                var weight = 1 + now.Subtract(request.QueuedOn).Seconds / SECONDS_PER_SCORE_WEIGHT_INCREASE;
                
                foreach (var preference in preferences)
                    scores[preference] += weight;
            }

            return scores;
        }
    }
}