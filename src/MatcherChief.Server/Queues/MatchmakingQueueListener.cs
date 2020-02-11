using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
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

        public MatchmakingQueueListener(GameFormat format, BlockingCollection<QueuedMatchRequestModel> queue)
        {
            _format = format;
            _queue = queue;
        }

        public async Task Listen(CancellationToken token)
        {
            // TODO: implement
            await Task.Delay(5000);
        }
    }
}