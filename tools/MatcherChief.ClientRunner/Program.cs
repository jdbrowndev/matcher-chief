using System;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Client;
using MatcherChief.Core.Models;
using MatcherChief.Shared;

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
                GameFormat = (int) GameFormat.OneVersusOne,
                GameTitles = new [] { (int) GameTitle.HaloReach },
                GameModes = new [] { (int) GameMode.Slayer }
            };

            var response = await client.GetMatch(request, CancellationToken.None);

            Console.WriteLine(response.MatchId);
        }
    }
}
