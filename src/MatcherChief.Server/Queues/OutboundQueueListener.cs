using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Server.WebSockets;

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

        public OutboundQueueListener(IQueueManager queueManager, IWebSocketResponseHandler responseHandler)
        {
            _queueManager = queueManager;
            _responseHandler = responseHandler;
        }

        // TODO: asynchrouously take from queue?
        public async Task Listen(CancellationToken token)
        {
            var outQueue = _queueManager.OutboundQueue;
            while (!token.IsCancellationRequested)
            {
                var response = outQueue.Take();
                await _responseHandler.Handle(response);
            }
        }
    }
}