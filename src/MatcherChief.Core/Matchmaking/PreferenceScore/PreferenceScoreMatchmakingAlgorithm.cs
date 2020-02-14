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

        // TODO: adjust output to return list of unmatched players
        public IEnumerable<Match> GetMatches(GameFormat format, IEnumerable<MatchRequest> requests)
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

                var requestGroup = new List<MatchRequest>();
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
                            var players = requestGroup.Select(x => x.Player).ToList();
                            requestGroup.ForEach(x => orderedRequests.Remove(x));

                            var match = new Match(format, preference.Title, preference.Mode, players);
                            matches.Add(match);

                            requestGroup.Clear();
                        }
                    }
                }
            }

            return matches;
        }

        private bool HasPreference(MatchRequest request, Preference preference)
        {
            return request.Titles.Contains(preference.Title) && request.Modes.Contains(preference.Mode);
        }
    }
}