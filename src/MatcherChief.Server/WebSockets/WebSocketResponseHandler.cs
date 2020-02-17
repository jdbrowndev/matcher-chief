using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Server.Models;

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
            var segments = SegmentBytes(bytes, bufferSize);

            var webSocket = response.WebSocket;
            var tcs = response.WebSocketCompletionSource;
            foreach (var segment in segments)
            {
                var eom = segment == segments.Last();
                await webSocket.SendAsync(segment, WebSocketMessageType.Text, eom, CancellationToken.None);
            }

            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Match response sent", CancellationToken.None);
            tcs.TrySetResult(new object());
        }

        private List<ArraySegment<byte>> SegmentBytes(byte[] bytes, int bytesPerSegment)
        {
            var segments = new List<ArraySegment<byte>>();

            var i = 0;
            var offset = 0;
            var count = bytesPerSegment;
            while (offset < bytes.Length)
            {
                if (offset + count > bytes.Length)
                    count = bytes.Length - offset;

                segments.Add(new ArraySegment<byte>(bytes, offset, count));

                i++;
                offset = i * bytesPerSegment;
                count = bytesPerSegment;
            }

            return segments;
        }
    }
}