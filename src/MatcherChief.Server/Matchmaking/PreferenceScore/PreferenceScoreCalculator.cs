using System;
using System.Collections.Generic;
using MatcherChief.Server.Matchmaking.Models;

namespace MatcherChief.Server.Matchmaking.PreferenceScore
{
    public interface IPreferenceScoreCalculator
    {
        Dictionary<Preference, int> GetScores(IEnumerable<MatchRequest> requests);
    }

    public class PreferenceScoreCalculator : IPreferenceScoreCalculator
    {
        public const int SECONDS_PER_SCORE_WEIGHT_INCREASE = 5;

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
                    if (scores.ContainsKey(preference))
                        scores[preference] += weight;
                    else
                        scores.Add(preference, weight);
            }

            return scores;
        }
    }
}