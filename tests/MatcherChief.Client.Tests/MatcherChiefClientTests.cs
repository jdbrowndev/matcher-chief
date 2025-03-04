using MatcherChief.Shared.Contract;
using MatcherChief.Shared.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MatcherChief.Client.Tests;

public class MatcherChiefClientTests
{
	private readonly MatcherChiefClient _sut;

	public MatcherChiefClientTests()
	{
		_sut = new MatcherChiefClient(new Uri("ws://localhost:48689"));
	}

	[Fact]
	public async Task Should_Require_At_Least_One_Player()
	{
		var request = new MatchRequestModel
		{
			Players = [],
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = [GameTitle.HaloReach],
			GameModes = [GameMode.Slayer]
		};

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetMatch(request, CancellationToken.None));
	}

	[Fact]
	public async Task Should_Restrict_Party_Size_To_The_Selected_GameFormat()
	{
		var player1 = new PlayerModel { Id = Guid.NewGuid(), Name = "bob" };
		var player2 = new PlayerModel { Id = Guid.NewGuid(), Name = "sue" };
		var player3 = new PlayerModel { Id = Guid.NewGuid(), Name = "james" };

		var request = new MatchRequestModel
		{
			Players = [player1, player2, player3],
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = [GameTitle.HaloReach],
			GameModes = [GameMode.Slayer]
		};

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetMatch(request, CancellationToken.None));
	}

	[Fact]
	public async Task Should_Require_Player_Id()
	{
		var player1 = new PlayerModel { Name = "bob" };
		var player2 = new PlayerModel { Id = Guid.NewGuid(), Name = "sue" };

		var request = new MatchRequestModel
		{
			Players = [player1, player2],
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = [GameTitle.HaloReach],
			GameModes = [GameMode.Slayer]
		};

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetMatch(request, CancellationToken.None));
	}

	[Fact]
	public async Task Should_Require_Player_Name()
	{
		var player1 = new PlayerModel { Id = Guid.NewGuid() };
		var player2 = new PlayerModel { Id = Guid.NewGuid(), Name = "sue" };

		var request = new MatchRequestModel
		{
			Players = [player1, player2],
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = [GameTitle.HaloReach],
			GameModes = [GameMode.Slayer]
		};

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetMatch(request, CancellationToken.None));
	}

	[Fact]
	public async Task Should_Require_At_Least_One_GameTitle()
	{
		var player1 = new PlayerModel { Id = Guid.NewGuid(), Name = "bob" };
		var player2 = new PlayerModel { Id = Guid.NewGuid(), Name = "sue" };

		var request = new MatchRequestModel
		{
			Players = [player1, player2],
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = [],
			GameModes = [GameMode.Slayer]
		};

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetMatch(request, CancellationToken.None));
	}

	[Fact]
	public async Task Should_Require_At_Least_One_GameMode()
	{
		var player1 = new PlayerModel { Id = Guid.NewGuid(), Name = "bob" };
		var player2 = new PlayerModel { Id = Guid.NewGuid(), Name = "sue" };

		var request = new MatchRequestModel
		{
			Players = [player1, player2],
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = [GameTitle.HaloReach],
			GameModes = []
		};

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetMatch(request, CancellationToken.None));
	}

	[Fact]
	public async Task Should_Restrict_GameTitles_To_The_Selected_GameFormat()
	{
		var player1 = new PlayerModel { Id = Guid.NewGuid(), Name = "bob" };
		var player2 = new PlayerModel { Id = Guid.NewGuid(), Name = "sue" };

		var request = new MatchRequestModel
		{
			Players = [player1, player2],
			GameFormat = GameFormat.FourPlayerFirefight,
			GameTitles = [GameTitle.HaloCE],
			GameModes = [GameMode.FirefightArcade]
		};

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetMatch(request, CancellationToken.None));
	}

	[Fact]
	public async Task Should_Restrict_GameModes_To_The_Selected_GameFormat()
	{
		var player1 = new PlayerModel { Id = Guid.NewGuid(), Name = "bob" };
		var player2 = new PlayerModel { Id = Guid.NewGuid(), Name = "sue" };

		var request = new MatchRequestModel
		{
			Players = [player1, player2],
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = [GameTitle.HaloReach],
			GameModes = [GameMode.FirefightArcade]
		};

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetMatch(request, CancellationToken.None));
	}
}
