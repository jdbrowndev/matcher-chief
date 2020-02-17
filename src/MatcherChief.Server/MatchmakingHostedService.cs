using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Server.Queues;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MatcherChief.Server
{
    public class MatchmakingHostedService : BackgroundService
    {
        private readonly IQueueManager _queueManager;
        private readonly IMatchmakingQueueListenerFactory _matchmakingQueueListenerFactory;
        private readonly IOutboundQueueListener _outboundQueueListener;
        private readonly ILogger<MatchmakingHostedService> _logger;
        
        public IEnumerable<Task> BackgroundTasks { get; private set; }

        public MatchmakingHostedService(IQueueManager queueManager, IMatchmakingQueueListenerFactory matchmakingQueueListenerFactory, IOutboundQueueListener outboundQueueListener,
            ILogger<MatchmakingHostedService> logger)
        {
            _queueManager = queueManager;
            _matchmakingQueueListenerFactory = matchmakingQueueListenerFactory;
            _outboundQueueListener = outboundQueueListener;
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Matchmaking starting...");
            await Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var tasks = new List<Task>();

            foreach (var format in _queueManager.GameFormatsToQueues.Keys)
            {
                var listener = _matchmakingQueueListenerFactory.Get(format);
                tasks.Add(listener.Listen(stoppingToken));
            }

            tasks.Add(_outboundQueueListener.Listen(stoppingToken));
            BackgroundTasks = tasks;

            await Task.WhenAll(BackgroundTasks);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Matchmaking stopping...");
            await Task.CompletedTask;
        }
    }
}