using System.Net.WebSockets;
using System.Threading.Tasks;

namespace MatcherChief.Server.WebSockets
{
    public interface IWebSocketResponseHandler
    {
        Task Handle(WebSocket socket, TaskCompletionSource<object> tcs);
    }

    public class WebSocketResponseHandler : IWebSocketResponseHandler
    {
        public async Task Handle(WebSocket socket, TaskCompletionSource<object> tcs)
        {
            // TODO: implement
            await Task.Delay(1000);
        }
    }
}