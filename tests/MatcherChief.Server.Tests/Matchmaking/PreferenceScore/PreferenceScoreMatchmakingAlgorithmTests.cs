using System;
using System.Collections.Generic;
using System.Linq;
using MatcherChief.Server.Matchmaking.Models;
using MatcherChief.Server.Matchmaking.PreferenceScore;
using MatcherChief.Shared.Enums;
using Moq;
using Xunit;

namespace MatcherChief.Server.Tests.Matchmaking.PreferenceScore;

public class PreferenceScoreMatchmakingAlgorithmTests
{
    private readonly Mock<IPreferenceScoreCalculator> _scoreCalculator;
    private readonly PreferenceScoreMatchmakingAlgorithm _sut;

    public PreferenceScoreMatchmakingAlgorithmTests()
    {
         _scoreCalculator = new Mock<IPreferenceScoreCalculator>();
        _sut = new PreferenceScoreMatchmakingAlgorithm(_scoreCalculator.Object);
    }

    [Fact]
    public void Should_Group_Players_From_Highest_Preference_Score_To_Lowest()
    {
        var now = DateTime.Now;
        
        var player1 = new[] { new Player("bob") };
        var player2 = new[] { new Player("sue") };
        var player3 = new[] { new Player("jerry") };

        var request1 = new MatchRequest(player1, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer, GameMode.Swat }, now + TimeSpan.FromSeconds(2));
        var request2 = new MatchRequest(player2, new [] { GameTitle.HaloReach, GameTitle.Halo3 }, new [] { GameMode.Slayer, GameMode.Swat }, now + TimeSpan.FromSeconds(1));
        var request3 = new MatchRequest(player3, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer }, now);

        var scores = new Dictionary<Preference, int>
        {
            { new Preference(GameTitle.HaloReach, GameMode.Swat), 2 },
            { new Preference(GameTitle.HaloReach, GameMode.Slayer), 3 },
            { new Preference(GameTitle.Halo3, GameMode.Slayer), 1 },
            { new Preference(GameTitle.Halo3, GameMode.Swat), 1 }
        };

        _scoreCalculator.Setup(x => x.GetScores(It.IsAny<IEnumerable<MatchRequest>>())).Returns(scores);
        var result = _sut.Matchmake(GameFormat.OneVersusOne, new [] { request1, request2, request3 });

        Assert.Single(result.Matches);
        var match = result.Matches.Single();

        Assert.Equal(GameFormat.OneVersusOne, match.Format);
        Assert.Equal(GameTitle.HaloReach, match.Title);
        Assert.Equal(GameMode.Slayer, match.Mode);
        Assert.Contains(request2, match.Requests);
        Assert.Contains(request3, match.Requests);

        Assert.Contains(request1, result.UnmatchedRequests);
    }

    [Fact]
    public void Should_Match_Players_With_Longest_Queued_Times_First()
    {
        var now = DateTime.Now;
        
        var player1 = new[] { new Player("bob") };
        var player2 = new[] { new Player("sue") };
        var player3 = new[] { new Player("jerry") };

        var request1 = new MatchRequest(player1, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer, GameMode.Swat }, now + TimeSpan.FromSeconds(2));
        var request2 = new MatchRequest(player2, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer, GameMode.Swat }, now + TimeSpan.FromSeconds(3));
        var request3 = new MatchRequest(player3, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer }, now);

        var scores = new Dictionary<Preference, int>
        {
            { new Preference(GameTitle.HaloReach, GameMode.Swat), 2 },
            { new Preference(GameTitle.HaloReach, GameMode.Slayer), 3 }
        };

        _scoreCalculator.Setup(x => x.GetScores(It.IsAny<IEnumerable<MatchRequest>>())).Returns(scores);
        var result = _sut.Matchmake(GameFormat.OneVersusOne, new [] { request1, request2, request3 });

        Assert.Single(result.Matches);
        var match = result.Matches.Single();
        
        Assert.Equal(GameFormat.OneVersusOne, match.Format);
        Assert.Equal(GameTitle.HaloReach, match.Title);
        Assert.Equal(GameMode.Slayer, match.Mode);
        Assert.Contains(request1, match.Requests);
        Assert.Contains(request3, match.Requests);

        Assert.Contains(request2, result.UnmatchedRequests);
    }

    [Fact]
    public void Should_Attempt_To_Match_Players_Until_Not_Possible()
    {
        var now = DateTime.Now;
        
        var player1 = new[] { new Player("bob") };
        var player2 = new[] { new Player("sue") };
        var player3 = new[] { new Player("jerry") };
        var player4 = new[] { new Player("james") };
        var player5 = new[] { new Player("mark") };

        var request1 = new MatchRequest(player1, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer }, now);
        var request2 = new MatchRequest(player2, new [] { GameTitle.HaloReach }, new [] { GameMode.Swat }, now + TimeSpan.FromSeconds(1));
        var request3 = new MatchRequest(player3, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer }, now + TimeSpan.FromSeconds(2));
        var request4 = new MatchRequest(player4, new [] { GameTitle.HaloReach }, new [] { GameMode.Swat }, now + TimeSpan.FromSeconds(3));
        var request5 = new MatchRequest(player5, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer }, now + TimeSpan.FromSeconds(4));

        var scores = new Dictionary<Preference, int>
        {
            { new Preference(GameTitle.HaloReach, GameMode.Swat), 2 },
            { new Preference(GameTitle.HaloReach, GameMode.Slayer), 3 }
        };

        _scoreCalculator.Setup(x => x.GetScores(It.IsAny<IEnumerable<MatchRequest>>())).Returns(scores);
        var result = _sut.Matchmake(GameFormat.OneVersusOne, new [] { request1, request2, request3, request4, request5 });
        var matches = result.Matches;

        Assert.Equal(2, matches.Count());
        var match1 = matches.ElementAt(0);
        
        Assert.Equal(GameFormat.OneVersusOne, match1.Format);
        Assert.Equal(GameTitle.HaloReach, match1.Title);
        Assert.Equal(GameMode.Slayer, match1.Mode);
        Assert.Contains(request1, match1.Requests);
        Assert.Contains(request3, match1.Requests);

        var match2 = matches.ElementAt(1);
        
        Assert.Equal(GameFormat.OneVersusOne, match2.Format);
        Assert.Equal(GameTitle.HaloReach, match2.Title);
        Assert.Equal(GameMode.Swat, match2.Mode);
        Assert.Contains(request2, match2.Requests);
        Assert.Contains(request4, match2.Requests);

        Assert.Contains(request5, result.UnmatchedRequests);
    }

    [Fact]
    public void Should_Match_Player_Party_In_Single_Request()
    {
        var now = DateTime.Now;
        
        var party1 = new[] { new Player("bob"), new Player("sue") };

        var request1 = new MatchRequest(party1, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer }, now);

        var scores = new Dictionary<Preference, int>
        {
            { new Preference(GameTitle.HaloReach, GameMode.Slayer), 1 }
        };

        _scoreCalculator.Setup(x => x.GetScores(It.IsAny<IEnumerable<MatchRequest>>())).Returns(scores);
        var result = _sut.Matchmake(GameFormat.OneVersusOne, new [] { request1 });
        var matches = result.Matches;

        Assert.Single(matches);
        var match1 = matches.ElementAt(0);
        
        Assert.Equal(GameFormat.OneVersusOne, match1.Format);
        Assert.Equal(GameTitle.HaloReach, match1.Title);
        Assert.Equal(GameMode.Slayer, match1.Mode);
        Assert.Contains(request1, match1.Requests);
    }

    [Fact]
    public void Should_Match_Player_Parties_In_Multiple_Requests()
    {
        var now = DateTime.Now;
        
        var party1 = new[] { new Player("bob"), new Player("sue") };
        var party2 = new[] { new Player("jerry"), new Player("james") };

        var request1 = new MatchRequest(party1, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer }, now);
        var request2 = new MatchRequest(party2, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer }, now);

        var scores = new Dictionary<Preference, int>
        {
            { new Preference(GameTitle.HaloReach, GameMode.Slayer), 2 }
        };

        _scoreCalculator.Setup(x => x.GetScores(It.IsAny<IEnumerable<MatchRequest>>())).Returns(scores);
        var result = _sut.Matchmake(GameFormat.TwoVersusTwo, new [] { request1, request2 });
        var matches = result.Matches;

        Assert.Single(matches);
        var match1 = matches.ElementAt(0);
        
        Assert.Equal(GameFormat.TwoVersusTwo, match1.Format);
        Assert.Equal(GameTitle.HaloReach, match1.Title);
        Assert.Equal(GameMode.Slayer, match1.Mode);
        Assert.Contains(request1, match1.Requests);
        Assert.Contains(request2, match1.Requests);
    }
}
