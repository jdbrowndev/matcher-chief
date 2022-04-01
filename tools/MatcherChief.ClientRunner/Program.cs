using MatcherChief.Client;
using MatcherChief.Shared;
using MatcherChief.Shared.Contract;
using MatcherChief.Shared.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MatcherChief.ClientRunner;

public class Program
{
	private static Random _random = new Random();
	private static GameFormat[] _gameFormats = (GameFormat[])Enum.GetValues(typeof(GameFormat));
	private static int _totalRequests = 0;

	public static async Task Main()
	{
		var config = GetConfig();
		var serverUrl = config["ClientRunner:ServerUrl"];
		var msDelay = int.Parse(config["ClientRunner:ConnectionDelay"]);

		Console.WriteLine($"ClientRunner starting up... (ServerUrl={serverUrl}, ConnectionDelay={msDelay})");

		_ = LogProgress();
		var uri = new Uri(serverUrl);
		var client = new MatcherChiefClient(uri);
		var tasks = new List<Task>();

		while (true)
		{
			try
			{
				tasks = await RefreshTasks(tasks);

				var request = GetRequest();
				var responseTask = client.GetMatch(request, CancellationToken.None);
				tasks.Add(responseTask);
				_totalRequests++;
			}
			catch (Exception exception)
			{
				Console.Error.WriteLine(exception);
			}
			finally
			{
				await Task.Delay(msDelay);
			}
		}
	}

	private static IConfiguration GetConfig()
	{
		var config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", true)
			.AddEnvironmentVariables()
			.Build();

		return config;
	}

	private static MatchRequestModel GetRequest()
	{
		var format = _gameFormats[_random.Next(_gameFormats.Length)];

		var isSolo = _random.Next(4) > 0;
		var partySize = isSolo ? 1 : _random.Next(GameSetup.GameFormatsToPlayersRequired[format]) + 1;
		var players = new List<PlayerModel>();
		for (var i = 0; i < partySize; i++)
		{
			var playerId = Guid.NewGuid();
			var player = new PlayerModel
			{
				Id = playerId,
				Name = $"player{playerId:N}"
			};
			players.Add(player);
		}

		var titles = new List<GameTitle>();
		var gameTitles = GameSetup.GameFormatsToTitles[format];
		var titleChance = _random.Next(gameTitles.Count());
		foreach (var title in gameTitles)
		{
			if (_random.Next(titleChance) == 0)
				titles.Add(title);
		}
		if (!titles.Any())
			titles.Add(gameTitles.First());

		var modes = new List<GameMode>();
		var gameModes = GameSetup.GameFormatsToModes[format];
		var modeChance = _random.Next(gameModes.Count());
		foreach (var mode in gameModes)
		{
			if (_random.Next(modeChance) == 0)
				modes.Add(mode);
		}
		if (!modes.Any())
			modes.Add(gameModes.First());

		var request = new MatchRequestModel
		{
			Players = players,
			GameFormat = format,
			GameTitles = titles,
			GameModes = modes
		};
		return request;
	}

	private static async Task<List<Task>> RefreshTasks(List<Task> tasks)
	{
		var completedTasks = tasks.Where(x => x.IsCompleted).ToList();
		var runningTasks = tasks.Where(x => !completedTasks.Contains(x)).ToList();

		foreach (var task in completedTasks)
		{
			try
			{
				await task;
			}
			catch (Exception exception)
			{
				Console.Error.WriteLine(exception);
			}
		}

		return runningTasks;
	}

	private static async Task LogProgress()
	{
		var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

		while (await timer.WaitForNextTickAsync())
		{
			Console.WriteLine($"Total requests sent: {_totalRequests}");
		}
	}
}
