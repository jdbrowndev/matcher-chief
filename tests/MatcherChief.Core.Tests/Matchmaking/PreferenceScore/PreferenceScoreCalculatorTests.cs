using System;
using MatcherChief.Core.Matchmaking.PreferenceScore;
using MatcherChief.Core.Models;
using Xunit;

namespace MatcherChief.Core.Tests.Matchmaking.PreferenceScore
{
    public class PreferenceScoreCalculatorTests
    {
        private PreferenceScoreCalculator _sut;

        public PreferenceScoreCalculatorTests()
        {
            _sut = new PreferenceScoreCalculator();
        }

        [Fact]
        public void Should_Return_Scores_For_All_Preferences_In_Requests()
        {
            var now = DateTime.Now;

            var player1 = new Player("bob");
            var player2 = new Player("sue");
            var player3 = new Player("jerry");

            var request1 = new MatchRequest(player1, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer, GameMode.Swat }, now);
            var request2 = new MatchRequest(player2, new [] { GameTitle.HaloReach, GameTitle.Halo2 }, new [] { GameMode.Slayer, GameMode.Snipers }, now);
            var request3 = new MatchRequest(player3, new [] { GameTitle.HaloReach }, new [] { GameMode.AssetDenial, GameMode.ActionSack }, now);

            var scores = _sut.GetScores(new[] { request1, request2, request3 });

            Assert.Equal(7, scores.Count);
            Assert.Equal(scores[new Preference(GameTitle.HaloReach, GameMode.Slayer)], 2);
            Assert.Equal(scores[new Preference(GameTitle.HaloReach, GameMode.Swat)], 1);
            Assert.Equal(scores[new Preference(GameTitle.HaloReach, GameMode.Snipers)], 1);
            Assert.Equal(scores[new Preference(GameTitle.Halo2, GameMode.Slayer)], 1);
            Assert.Equal(scores[new Preference(GameTitle.Halo2, GameMode.Snipers)], 1);
            Assert.Equal(scores[new Preference(GameTitle.HaloReach, GameMode.AssetDenial)], 1);
            Assert.Equal(scores[new Preference(GameTitle.HaloReach, GameMode.ActionSack)], 1);
        }

        [Fact]
        public void Should_Return_Weighted_Scores_Based_On_Queued_Time()
        {
            var now = DateTime.Now;

            var player1 = new Player("bob");
            var player2 = new Player("sue");
            var player3 = new Player("jerry");

            var request1QueuedOn = now - TimeSpan.FromSeconds(2.75);
            var request1 = new MatchRequest(player1, new [] { GameTitle.HaloReach }, new [] { GameMode.Slayer, GameMode.Swat }, request1QueuedOn);

            var request2QueuedOn = now - TimeSpan.FromSeconds(6);
            var request2 = new MatchRequest(player2, new [] { GameTitle.HaloReach, GameTitle.Halo2 }, new [] { GameMode.Slayer, GameMode.Snipers }, request2QueuedOn);

            var request3QueuedOn = now - TimeSpan.FromSeconds(14);
            var request3 = new MatchRequest(player3, new [] { GameTitle.HaloReach }, new [] { GameMode.AssetDenial, GameMode.ActionSack }, request3QueuedOn);

            var scores = _sut.GetScores(new[] { request1, request2, request3 });

            Assert.Equal(7, scores.Count);
            Assert.Equal(scores[new Preference(GameTitle.HaloReach, GameMode.Slayer)], 3);
            Assert.Equal(scores[new Preference(GameTitle.HaloReach, GameMode.Swat)], 1);
            Assert.Equal(scores[new Preference(GameTitle.HaloReach, GameMode.Snipers)], 2);
            Assert.Equal(scores[new Preference(GameTitle.Halo2, GameMode.Slayer)], 2);
            Assert.Equal(scores[new Preference(GameTitle.Halo2, GameMode.Snipers)], 2);
            Assert.Equal(scores[new Preference(GameTitle.HaloReach, GameMode.AssetDenial)], 3);
            Assert.Equal(scores[new Preference(GameTitle.HaloReach, GameMode.ActionSack)], 3);
        }
    }
}