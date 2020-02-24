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
        private const int BUFFER_SIZE = 1024 * 4;
        private readonly Uri _matchmakingServer;

        public MatcherChiefClient(Uri matchmakingServer)
        {
            _matchmakingServer = matchmakingServer;
        }

        public async Task<MatchResponseModel> GetMatch(MatchRequestModel request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            using (var webSocket = new ClientWebSocket())
            {
                await SendRequest(request, webSocket, cancellationToken);
                var response = await ReceiveResponse(webSocket, cancellationToken);
                await CloseHandshake(webSocket, cancellationToken);
                return response;
            }
        }

        private void ValidateRequest(MatchRequestModel request)
        {
            if (request.Players.Any(x => x.Id == Guid.Empty))
                throw new ArgumentException("PlayerId required");
            if (request.Players.Any(x => string.IsNullOrWhiteSpace(x.Name)))
                throw new ArgumentException("PlayerName required");
            if (request.GameTitles == null || !request.GameTitles.Any())
                throw new ArgumentException("At least one GameTitle required");
            if (request.GameModes == null || !request.GameModes.Any())
                throw new ArgumentException("At least one GameMode required");
            if (request.Players.Count() > GameSetup.GameFormatsToPlayersRequired[request.GameFormat])
                throw new ArgumentException("Too many players for the selected GameFormat");
            if (request.GameModes.Except(GameSetup.GameFormatsToModes[request.GameFormat]).Count() != 0)
                throw new ArgumentException("All GameModes must be valid for the selected GameFormat");
        }

        private async Task SendRequest(MatchRequestModel request, ClientWebSocket webSocket, CancellationToken cancellationToken)
        {
            var json = JsonSerializer.Serialize(request);
            var bytes = Encoding.UTF8.GetBytes(json);
            var segments = ArraySegmentHelper.Segment(bytes, BUFFER_SIZE);

            await webSocket.ConnectAsync(new Uri(_matchmakingServer, "/ws"), cancellationToken);
            foreach (var segment in segments)
            {
                var eom = segment == segments.Last();
                await webSocket.SendAsync(segment, WebSocketMessageType.Text, eom, CancellationToken.None);
            }
        }

        private async Task<MatchResponseModel> ReceiveResponse(ClientWebSocket webSocket, CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();
            var buffer = new byte[BUFFER_SIZE];
            WebSocketReceiveResult receiveResult;

            do
            {
                receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                sb.Append(Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
            } while (!receiveResult.EndOfMessage);

            var response = sb.ToString();
            var model = JsonSerializer.Deserialize<MatchResponseModel>(response);
            return model;
        }

        private async Task CloseHandshake(ClientWebSocket webSocket, CancellationToken cancellationToken)
        {
            var buffer = new byte[BUFFER_SIZE];
            var closeResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            await webSocket.CloseAsync(closeResult.CloseStatus.Value, closeResult.CloseStatusDescription, cancellationToken);
        }
    }
}