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
        private static int _playerCount = 0;
        private static GameFormat[] _gameFormats = (GameFormat[]) Enum.GetValues(typeof(GameFormat));
        private static GameTitle[] _gameTitles = (GameTitle[]) Enum.GetValues(typeof(GameTitle));

        public static async Task Main(string[] args)
        {
            // TODO: clean up
            var uri = new Uri("ws://localhost:48689");

            var tasks = new List<Task>();
            while (true)
            {
                var client = new MatcherChiefClient(uri);
                var request = GetRequest();
                var responseTask = client.GetMatch(request, CancellationToken.None);
                tasks.Add(responseTask);
                await Task.Delay(250);
            }
        }

        private static MatchRequestModel GetRequest()
        {
            var format = _gameFormats[_random.Next(_gameFormats.Length)];

            var titles = new List<GameTitle>();
            var titleChance = _random.Next(_gameTitles.Length);
            foreach (var title in _gameTitles)
            {
                if (_random.Next(titleChance) == 0)
                    titles.Add(title);
            }
            if (!titles.Any())
                titles.Add(GameTitle.HaloCE);

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
                PlayerId = Guid.NewGuid(),
                PlayerName = "player" + _playerCount++,
                GameFormat = format,
                GameTitles = titles,
                GameModes = modes
            };
            return request;
        }
    }
}
