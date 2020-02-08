using System.Collections.Generic;
using System.Linq;
using MatcherChief.Core.Models;

namespace MatcherChief.Core.Matchmaking.PreferenceScore
{
    public class PreferenceScoreMatchmakingAlgorithm : IMatchmakingAlgorithm
    {
        private readonly IPreferenceScoreCalculator _scoreCalculator;

        public PreferenceScoreMatchmakingAlgorithm(IPreferenceScoreCalculator scoreCalculator)
        {
            _scoreCalculator = scoreCalculator;
        }

        public IEnumerable<Match> GetMatches(GameFormat format, IEnumerable<MatchRequest> requests)
        {
            var score = _scoreCalculator.GetScores(requests);
            var orderedRequests = requests.OrderByDescending(x => x.QueuedOn).ToList();

            for (var i = orderedRequests.Count - 1; i >= 0; i--)
            {
                var request = orderedRequests[i];
            }

            // TODO: implement
            return new List<Match>();
        }
    }
}