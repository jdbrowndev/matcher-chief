using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Core.Matchmaking;
using MatcherChief.Core.Models;
using MatcherChief.Server.Models;

namespace MatcherChief.Server.Queues
{
    public interface IMatchmakingQueueListener
    {
        Task Listen(CancellationToken token);
    }

    public class MatchmakingQueueListener : IMatchmakingQueueListener
    {
        private readonly GameFormat _format;
        private readonly BlockingCollection<QueuedMatchRequestModel> _queue;
        private readonly IMatchmakingAlgorithm _matchMakingAlgorithm;

        public MatchmakingQueueListener(GameFormat format, BlockingCollection<QueuedMatchRequestModel> queue,
            IMatchmakingAlgorithm matchMakingAlgorithm)
        {
            _format = format;
            _queue = queue;
            _matchMakingAlgorithm = matchMakingAlgorithm;
        }

        public async Task Listen(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var request = _queue.Take();
                await Task.Delay(1000);
            }
        }
    }
}