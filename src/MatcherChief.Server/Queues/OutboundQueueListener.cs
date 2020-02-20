using System;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Server.WebSockets;
using Microsoft.Extensions.Logging;

namespace MatcherChief.Server.Queues
{
    public interface IOutboundQueueListener
    {
        Task Listen(CancellationToken token);
    }

    public class OutboundQueueListener : IOutboundQueueListener
    {
        private readonly IQueueManager _queueManager;
        private readonly IWebSocketResponseHandler _responseHandler;
        private readonly ILogger<OutboundQueueListener> _logger;

        public OutboundQueueListener(IQueueManager queueManager, IWebSocketResponseHandler responseHandler, ILogger<OutboundQueueListener> logger)
        {
            _queueManager = queueManager;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task Listen(CancellationToken token)
        {
            var outQueue = _queueManager.OutboundQueue;
            _logger.LogInformation($"OutboundQueueListener listening...");

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var response = await outQueue.DequeueAsync(token);
                    await _responseHandler.Handle(response);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("OutboundQueueListener shutting down...");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "OutboundQueueListener error");
                }
            }
        }
    }
}