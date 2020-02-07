using System.Collections.Generic;
using MatcherChief.Core.Models;

namespace MatcherChief.Core
{
    public interface IMatchmakingAlgorithm
    {
         IEnumerable<Match> GetMatches(IEnumerable<MatchRequest> requests);
    }
}