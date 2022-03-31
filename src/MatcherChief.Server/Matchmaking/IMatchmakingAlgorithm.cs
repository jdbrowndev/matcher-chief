using MatcherChief.Server.Matchmaking.Models;
using MatcherChief.Shared.Enums;
using System.Collections.Generic;

namespace MatcherChief.Server.Matchmaking;

public interface IMatchmakingAlgorithm
{
	MatchmakeResult Matchmake(GameFormat format, IEnumerable<MatchRequest> requests);
}
