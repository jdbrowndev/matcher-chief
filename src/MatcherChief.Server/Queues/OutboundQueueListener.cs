using System.Threading.Tasks;
using MatcherChief.Server.WebSockets;

namespace MatcherChief.Server.Queues
{
    public interface IOutboundQueueListener
    {
        Task Listen();
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

        public async Task Listen()
        {
            // TODO: implement
            await Task.Delay(5000);
        }
    }
}