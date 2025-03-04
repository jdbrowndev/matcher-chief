using MatcherChief.Server.Matchmaking.Models;
using MatcherChief.Server.Matchmaking.PreferenceScore;
using MatcherChief.Shared.Enums;
using System;
using Xunit;

namespace MatcherChief.Server.Tests.Matchmaking.PreferenceScore;

public class PreferenceScoreCalculatorTests
{
	private readonly PreferenceScoreCalculator _sut;

	public PreferenceScoreCalculatorTests()
	{
		_sut = new PreferenceScoreCalculator();
	}

	[Fact]
	public void Should_Return_Scores_For_All_Preferences_In_Requests()
	{
		var now = DateTime.Now;

		var player1 = new[] { new Player("bob") };
		var player2 = new[] { new Player("sue") };
		var player3 = new[] { new Player("jerry") };

		var request1 = new MatchRequest(player1, [GameTitle.HaloReach], [GameMode.Slayer, GameMode.Swat], now);
		var request2 = new MatchRequest(player2, [GameTitle.HaloReach, GameTitle.Halo2], [GameMode.Slayer, GameMode.Snipers], now);
		var request3 = new MatchRequest(player3, [GameTitle.HaloReach], [GameMode.AssetDenial, GameMode.ActionSack], now);

		var scores = _sut.GetScores([request1, request2, request3]);

		Assert.Equal(7, scores.Count);
		Assert.Equal(2, scores[new Preference(GameTitle.HaloReach, GameMode.Slayer)]);
		Assert.Equal(1, scores[new Preference(GameTitle.HaloReach, GameMode.Swat)]);
		Assert.Equal(1, scores[new Preference(GameTitle.HaloReach, GameMode.Snipers)]);
		Assert.Equal(1, scores[new Preference(GameTitle.Halo2, GameMode.Slayer)]);
		Assert.Equal(1, scores[new Preference(GameTitle.Halo2, GameMode.Snipers)]);
		Assert.Equal(1, scores[new Preference(GameTitle.HaloReach, GameMode.AssetDenial)]);
		Assert.Equal(1, scores[new Preference(GameTitle.HaloReach, GameMode.ActionSack)]);
	}

	[Fact]
	public void Should_Return_Weighted_Scores_Based_On_Queued_Time()
	{
		var now = DateTime.Now;

		var player1 = new[] { new Player("bob") };
		var player2 = new[] { new Player("sue") };
		var player3 = new[] { new Player("jerry") };

		var request1QueuedOn = now - TimeSpan.FromSeconds(2.75);
		var request1 = new MatchRequest(player1, [GameTitle.HaloReach], [GameMode.Slayer, GameMode.Swat], request1QueuedOn);

		var request2QueuedOn = now - TimeSpan.FromSeconds(6);
		var request2 = new MatchRequest(player2, [GameTitle.HaloReach, GameTitle.Halo2], [GameMode.Slayer, GameMode.Snipers], request2QueuedOn);

		var request3QueuedOn = now - TimeSpan.FromSeconds(14);
		var request3 = new MatchRequest(player3, [GameTitle.HaloReach], [GameMode.AssetDenial, GameMode.ActionSack], request3QueuedOn);

		var scores = _sut.GetScores([request1, request2, request3]);

		Assert.Equal(7, scores.Count);
		Assert.Equal(3, scores[new Preference(GameTitle.HaloReach, GameMode.Slayer)]);
		Assert.Equal(1, scores[new Preference(GameTitle.HaloReach, GameMode.Swat)]);
		Assert.Equal(2, scores[new Preference(GameTitle.HaloReach, GameMode.Snipers)]);
		Assert.Equal(2, scores[new Preference(GameTitle.Halo2, GameMode.Slayer)]);
		Assert.Equal(2, scores[new Preference(GameTitle.Halo2, GameMode.Snipers)]);
		Assert.Equal(3, scores[new Preference(GameTitle.HaloReach, GameMode.AssetDenial)]);
		Assert.Equal(3, scores[new Preference(GameTitle.HaloReach, GameMode.ActionSack)]);
	}
}
