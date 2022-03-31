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
			Players = new PlayerModel[0],
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = new[] { GameTitle.HaloReach },
			GameModes = new[] { GameMode.Slayer }
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
			Players = new[] { player1, player2, player3 },
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = new[] { GameTitle.HaloReach },
			GameModes = new[] { GameMode.Slayer }
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
			Players = new[] { player1, player2 },
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = new[] { GameTitle.HaloReach },
			GameModes = new[] { GameMode.Slayer }
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
			Players = new[] { player1, player2 },
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = new[] { GameTitle.HaloReach },
			GameModes = new[] { GameMode.Slayer }
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
			Players = new[] { player1, player2 },
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = new GameTitle[0],
			GameModes = new[] { GameMode.Slayer }
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
			Players = new[] { player1, player2 },
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = new[] { GameTitle.HaloReach },
			GameModes = new GameMode[0]
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
			Players = new[] { player1, player2 },
			GameFormat = GameFormat.OneVersusOne,
			GameTitles = new[] { GameTitle.HaloReach },
			GameModes = new[] { GameMode.FirefightArcade }
		};

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetMatch(request, CancellationToken.None));
	}
}
