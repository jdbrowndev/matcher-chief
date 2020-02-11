using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Core.Models;

namespace MatcherChief.Server.Queues
{
    public interface IMatchmakingQueueListener
    {
        Task Listen(CancellationToken token);
    }

    public class MatchmakingQueueListener : IMatchmakingQueueListener
    {
        private readonly GameFormat _format;
        private readonly BlockingCollection<MatchRequest> _queue;

        public MatchmakingQueueListener(GameFormat format, BlockingCollection<MatchRequest> queue)
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