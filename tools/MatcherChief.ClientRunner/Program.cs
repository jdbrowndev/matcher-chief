using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Client;
using MatcherChief.Shared;
using MatcherChief.Shared.Contract;
using MatcherChief.Shared.Enums;

namespace MatcherChief.ClientRunner
{
    public class Program
    {
        private static Random _random = new Random();
        private static GameFormat[] _gameFormats = (GameFormat[]) Enum.GetValues(typeof(GameFormat));
        private static GameTitle[] _gameTitles = (GameTitle[]) Enum.GetValues(typeof(GameTitle));

        public static async Task Main(string[] args)
        {
            var msDelay = 50;
            if (args.Length > 0)
                msDelay = int.Parse(args[0]);

            var uriString = "ws://localhost:48689";
            if (args.Length > 1)
                uriString = args[1];

            var uri = new Uri(uriString);
            var tasks = new List<Task>();
            while (true)
            {
                var client = new MatcherChiefClient(uri);
                var request = GetRequest();
                var responseTask = client.GetMatch(request, CancellationToken.None);
                tasks = tasks.Where(x => !x.IsCompleted).Concat(new [] { responseTask }).ToList();
                await Task.Delay(msDelay);
            }
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
            var titleChance = _random.Next(_gameTitles.Length);
            foreach (var title in _gameTitles)
            {
                if (_random.Next(titleChance) == 0)
                    titles.Add(title);
            }
            if (!titles.Any())
                titles.Add(_gameTitles.First());

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
    }
}
