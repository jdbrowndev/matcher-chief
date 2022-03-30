using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Server.Queues.Models;
using MatcherChief.Shared;
using MatcherChief.Shared.Contract;

namespace MatcherChief.Server.WebSockets;

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
            GameFormat = response.Match.Format,
            GameTitle = response.Match.Title,
            GameMode = response.Match.Mode,
            Players = response.Match.Requests.SelectMany(x => x.Players.Select(p => new PlayerModel { Id = p.Id, Name = p.Name }))
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
