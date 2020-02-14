using MatcherChief.Core.Matchmaking;
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
        private readonly IMatchmakingAlgorithm _matchmakingAlgorithm;

        public MatchmakingQueueListenerFactory(IQueueManager queueManager, IMatchmakingAlgorithm matchmakingAlgorithm)
        {
            _queueManager = queueManager;
            _matchmakingAlgorithm = matchmakingAlgorithm;
        }

        public MatchmakingQueueListener Get(GameFormat format)
        {
            var queue = _queueManager.GameFormatsToQueues[format];
            var listener = new MatchmakingQueueListener(format, queue, _matchmakingAlgorithm);
            return listener;
        }
    }
}