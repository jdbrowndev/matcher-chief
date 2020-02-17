using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Server.Models;
using MatcherChief.Shared;

namespace MatcherChief.Server.WebSockets
{
    public interface IWebSocketResponseHandler
    {
        Task Handle(QueuedMatchResponseModel response);
    }

    public class WebSocketResponseHandler : IWebSocketResponseHandler
    {
        public async Task Handle(QueuedMatchResponseModel response)
        {
            var responseModel = new MatchResponseModel
            {
                MatchId = response.Match.Id,
                GameFormat = (int) response.Match.Format,
                GameTitle = (int) response.Match.Title,
                GameMode = (int) response.Match.Mode,
                Players = response.Match.Requests.Select(x => new MatchResponsePlayerModel { PlayerId = x.Player.Id, PlayerName = x.Player.Name })
            };

            var json = JsonSerializer.Serialize(responseModel);
            var bytes = Encoding.UTF8.GetBytes(json);
            var bufferSize = 1024 * 4;
            var segments = ArraySegmentHelper.Segment(bytes, bufferSize);

            var webSocket = response.WebSocket;
            var tcs = response.WebSocketCompletionSource;
            foreach (var segment in segments)
            {
                var eom = segment == segments.Last();
                await webSocket.SendAsync(segment, WebSocketMessageType.Text, eom, CancellationToken.None);
            }

            tcs.TrySetResult(new object());
        }
    }
}