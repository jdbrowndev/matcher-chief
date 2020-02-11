using System.Threading.Tasks;

namespace MatcherChief.Server.Queues
{
    public interface IOutboundQueueListener
    {
        Task Listen();
    }

    public class OutboundQueueListener : IOutboundQueueListener
    {
        private readonly IQueueManager _queueManager;

        public OutboundQueueListener(IQueueManager queueManager)
        {
            _queueManager = queueManager;
        }

        public async Task Listen()
        {
            // TODO: implement
            await Task.Delay(5000);
        }
    }
}