using System.Collections.Generic;
using MatcherChief.Server.Matchmaking.Models;
using MatcherChief.Shared.Enums;

namespace MatcherChief.Server.Matchmaking
{
    public interface IMatchmakingAlgorithm
    {
         MatchmakeResult Matchmake(GameFormat format, IEnumerable<MatchRequest> requests);
    }
}