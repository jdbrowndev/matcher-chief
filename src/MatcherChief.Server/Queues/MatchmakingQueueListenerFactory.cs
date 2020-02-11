using MatcherChief.Core.Models;

namespace MatcherChief.Server.Queues
{
    public interface IMatchmakingQueueListenerFactory
    {
        MatchmakingQueueListener Get(GameFormat format);
    }

    public class MatchmakingQueueListenerFactory : IMatchmakingQueueListenerFactory
    {
        private readonly IQueueManager _queueManager;

        public MatchmakingQueueListenerFactory(IQueueManager queueManager)
        {
            _queueManager = queueManager;
        }

        public MatchmakingQueueListener Get(GameFormat format)
        {
            var queue = _queueManager.GameFormatsToQueues[format];
            var listener = new MatchmakingQueueListener(format, queue);
            return listener;
        }
    }
}