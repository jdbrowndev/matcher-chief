using System.Net.WebSockets;
using System.Threading.Tasks;

namespace MatcherChief.Server.WebSockets
{
    public interface IWebSocketRequestHandler
    {
        Task Handle(WebSocket socket, TaskCompletionSource<object> tcs);
    }

    public class WebSocketRequestHandler : IWebSocketRequestHandler
    {
        public async Task Handle(WebSocket socket, TaskCompletionSource<object> tcs)
        {
            // TODO: implement
            await Task.Delay(1000);
        }
    }
}