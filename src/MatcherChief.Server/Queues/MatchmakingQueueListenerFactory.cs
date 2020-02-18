using MatcherChief.Core.Matchmaking;
using MatcherChief.Core.Models;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<MatchmakingQueueListener> _logger;

        public MatchmakingQueueListenerFactory(IQueueManager queueManager, IMatchmakingAlgorithm matchmakingAlgorithm, ILogger<MatchmakingQueueListener> logger)
        {
            _queueManager = queueManager;
            _matchmakingAlgorithm = matchmakingAlgorithm;
            _logger = logger;
        }

        public MatchmakingQueueListener Get(GameFormat format)
        {
            var inQueue = _queueManager.GameFormatsToQueues[format];
            var outQueue = _queueManager.OutboundQueue;
            var listener = new MatchmakingQueueListener(format, inQueue, outQueue, _matchmakingAlgorithm, _logger);
            return listener;
        }
    }
}