using System.Collections.Generic;
using MatcherChief.Core.Models;

namespace MatcherChief.Core.Matchmaking
{
    public interface IMatchmakingAlgorithm
    {
         MatchmakeResult Matchmake(GameFormat format, IEnumerable<MatchRequest> requests);
    }
}