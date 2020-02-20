using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using MatcherChief.Shared;
using System.Text;
using System.Linq;
using MatcherChief.Shared.Contract;

namespace MatcherChief.Client
{
    public class MatcherChiefClient
    {
        private readonly Uri _matchmakingServer;

        public MatcherChiefClient(Uri matchmakingServer)
        {
            _matchmakingServer = matchmakingServer;
        }

        public async Task<MatchResponseModel> GetMatch(MatchRequestModel request, CancellationToken cancellationToken)
        {
            using (var webSocket = new ClientWebSocket())
            {
                var json = JsonSerializer.Serialize(request);
                var bytes = Encoding.UTF8.GetBytes(json);
                var bufferSize = 1024 * 4;
                var segments = ArraySegmentHelper.Segment(bytes, bufferSize);

                await webSocket.ConnectAsync(new Uri(_matchmakingServer, "/ws"), cancellationToken);
                foreach (var segment in segments)
                {
                    var eom = segment == segments.Last();
                    await webSocket.SendAsync(segment, WebSocketMessageType.Text, eom, CancellationToken.None);
                }

                var sb = new StringBuilder();
                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult receiveResult;

                do
                {
                    receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                    sb.Append(Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
                } while (!receiveResult.EndOfMessage);

                var response = sb.ToString();
                var model = JsonSerializer.Deserialize<MatchResponseModel>(response);

                var closeResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                await webSocket.CloseAsync(closeResult.CloseStatus.Value, closeResult.CloseStatusDescription, cancellationToken);

                return model;
            }
        }
    }
}