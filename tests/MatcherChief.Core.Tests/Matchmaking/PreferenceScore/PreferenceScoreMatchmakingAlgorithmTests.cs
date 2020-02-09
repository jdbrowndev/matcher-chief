using System;
using System.Collections.Generic;
using System.Linq;
using MatcherChief.Core.Matchmaking.PreferenceScore;
using MatcherChief.Core.Models;
using Moq;
using Xunit;

namespace MatcherChief.Core.Tests.Matchmaking.PreferenceScore
{
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
            
            var player1 = new Player("bob");
            var player2 = new Player("sue");
            var player3 = new Player("jerry");

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
            var matches = _sut.GetMatches(GameFormat.OneVersusOne, new [] { request1, request2, request3 });

            Assert.Single(matches);
            var match = matches.Single();

            Assert.Equal(GameFormat.OneVersusOne, match.Format);
            Assert.Equal(GameTitle.HaloReach, match.Title);
            Assert.Equal(GameMode.Slayer, match.Mode);
            Assert.Contains(player2, match.Players);
            Assert.Contains(player3, match.Players);
        }

        [Fact]
        public void Should_Match_Players_With_Longest_Queued_Times_First()
        {
            var now = DateTime.Now;
            
            var player1 = new Player("bob");
            var player2 = new Player("sue");
            var player3 = new Player("jerry");

            var request1 = new MatchRequest(player1, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer, GameMode.Swat }, now + TimeSpan.FromSeconds(2));
            var request2 = new MatchRequest(player2, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer, GameMode.Swat }, now + TimeSpan.FromSeconds(3));
            var request3 = new MatchRequest(player3, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer }, now);

            var scores = new Dictionary<Preference, int>
            {
                { new Preference(GameTitle.HaloReach, GameMode.Swat), 2 },
                { new Preference(GameTitle.HaloReach, GameMode.Slayer), 3 }
            };

            _scoreCalculator.Setup(x => x.GetScores(It.IsAny<IEnumerable<MatchRequest>>())).Returns(scores);
            var matches = _sut.GetMatches(GameFormat.OneVersusOne, new [] { request1, request2, request3 });

            Assert.Single(matches);
            var match = matches.Single();
            
            Assert.Equal(GameFormat.OneVersusOne, match.Format);
            Assert.Equal(GameTitle.HaloReach, match.Title);
            Assert.Equal(GameMode.Slayer, match.Mode);
            Assert.Contains(player1, match.Players);
            Assert.Contains(player3, match.Players);
        }

        [Fact]
        public void Should_Attempt_To_Match_Players_Until_Not_Possible()
        {
            var now = DateTime.Now;
            
            var player1 = new Player("bob");
            var player2 = new Player("sue");
            var player3 = new Player("jerry");
            var player4 = new Player("james");
            var player5 = new Player("mark");

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
            var matches = _sut.GetMatches(GameFormat.OneVersusOne, new [] { request1, request2, request3, request4, request5 });

            Assert.Equal(2, matches.Count());
            var match1 = matches.ElementAt(0);
            
            Assert.Equal(GameFormat.OneVersusOne, match1.Format);
            Assert.Equal(GameTitle.HaloReach, match1.Title);
            Assert.Equal(GameMode.Slayer, match1.Mode);
            Assert.Contains(player1, match1.Players);
            Assert.Contains(player3, match1.Players);

            var match2 = matches.ElementAt(1);
            
            Assert.Equal(GameFormat.OneVersusOne, match2.Format);
            Assert.Equal(GameTitle.HaloReach, match2.Title);
            Assert.Equal(GameMode.Swat, match2.Mode);
            Assert.Contains(player2, match2.Players);
            Assert.Contains(player4, match2.Players);
        }
    }
}