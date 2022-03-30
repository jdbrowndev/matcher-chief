using System.Collections.Generic;

namespace MatcherChief.Server.Matchmaking.Models;

public class MatchmakeResult
{
    public MatchmakeResult(IEnumerable<Match> matches, IEnumerable<MatchRequest> unmatchedRequests)
    {
        Matches = matches;
        UnmatchedRequests = unmatchedRequests;
    }

    public IEnumerable<Match> Matches { get; private set; }
    public IEnumerable<MatchRequest> UnmatchedRequests { get; private set; }
}
