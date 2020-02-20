using System;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Client;
using MatcherChief.Shared.Contract;
using MatcherChief.Shared.Enums;

namespace MatcherChief.ClientRunner
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // TODO: clean up
            var uri = new Uri("ws://localhost:48689");
            var client = new MatcherChiefClient(uri);
            var random = new Random();
            
            var request = new MatchRequestModel
            {
                PlayerId = Guid.NewGuid(),
                PlayerName = "player" + random.Next(10000),
                GameFormat = GameFormat.OneVersusOne,
                GameTitles = new [] { GameTitle.HaloReach },
                GameModes = new [] { GameMode.Slayer }
            };

            var response = await client.GetMatch(request, CancellationToken.None);

            Console.WriteLine(response.MatchId);
        }
    }
}
