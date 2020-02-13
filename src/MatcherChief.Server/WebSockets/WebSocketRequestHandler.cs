using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Server.Models;
using MatcherChief.Server.Queues;

namespace MatcherChief.Server.WebSockets
{
    public interface IWebSocketRequestHandler
    {
        Task Handle(WebSocket socket, TaskCompletionSource<object> tcs);
    }

    public class WebSocketRequestHandler : IWebSocketRequestHandler
    {
        private readonly IQueueManager _queueManager;

        public WebSocketRequestHandler(IQueueManager queueManager)
        {
            _queueManager = queueManager;
        }

        public async Task Handle(WebSocket webSocket, TaskCompletionSource<object> tcs)
        {
            var sb = new StringBuilder();
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult receiveResult;

            do
            {
                receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                sb.Append(Encoding.UTF8.GetString(buffer));
            } while (!receiveResult.CloseStatus.HasValue);

            var request = sb.ToString();
            var model = JsonSerializer.Deserialize<MatchRequestModel>(request);

            var queue = _queueManager.GameFormatsToQueues[model.Format];
            var queuedMatchRequest = new QueuedMatchRequestModel
            {
                WebSocket = webSocket,
                CompletionSource = tcs
                // TODO: assign more props
            };
            queue.Add(queuedMatchRequest);
        }
    }
}