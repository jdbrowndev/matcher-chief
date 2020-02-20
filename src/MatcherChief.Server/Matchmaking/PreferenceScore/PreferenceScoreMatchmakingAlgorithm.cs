using System.Collections.Generic;
using System.Linq;
using MatcherChief.Server.Matchmaking.Models;
using MatcherChief.Shared;
using MatcherChief.Shared.Enums;

namespace MatcherChief.Server.Matchmaking.PreferenceScore
{
    public class PreferenceScoreMatchmakingAlgorithm : IMatchmakingAlgorithm
    {
        private readonly IPreferenceScoreCalculator _scoreCalculator;

        public PreferenceScoreMatchmakingAlgorithm(IPreferenceScoreCalculator scoreCalculator)
        {
            _scoreCalculator = scoreCalculator;
        }

        public MatchmakeResult Matchmake(GameFormat format, IEnumerable<MatchRequest> requests)
        {
            var matchSize = GameSetup.GameFormatsToPlayersRequired[format];
            var matches = new List<Match>();

            var preferenceScores = _scoreCalculator.GetScores(requests);
            var orderedPreferences = preferenceScores
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .ToList();

            var orderedRequests = requests.OrderByDescending(x => x.QueuedOn).ToList();

            foreach (var preference in orderedPreferences)
            {
                if (orderedRequests.Count < matchSize)
                    break;

                var requestGroup = new List<MatchRequest>( matchSize );
                for (var i = orderedRequests.Count - 1; i >= 0; i--)
                {
                    if (i + 1 + requestGroup.Count < matchSize)
                        break;

                    var request = orderedRequests[i];
                    if (HasPreference(request, preference))
                    {
                        requestGroup.Add(request);
                        if (requestGroup.Count == matchSize)
                        {
                            requestGroup.ForEach(x => orderedRequests.Remove(x));

                            var match = new Match(requestGroup, format, preference.Title, preference.Mode);
                            matches.Add(match);

                            requestGroup = new List<MatchRequest>( matchSize );
                        }
                    }
                }
            }

            var result = new MatchmakeResult(matches, orderedRequests);
            return result;
        }

        private bool HasPreference(MatchRequest request, Preference preference)
        {
            return request.Titles.Contains(preference.Title) && request.Modes.Contains(preference.Mode);
        }
    }
}