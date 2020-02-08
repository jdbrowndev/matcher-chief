using System.Collections.Generic;
using MatcherChief.Core.Models;

namespace MatcherChief.Core.Matchmaking
{
    public interface IMatchmakingAlgorithm
    {
         IEnumerable<Match> GetMatches(GameFormat format, IEnumerable<MatchRequest> requests);
    }
}