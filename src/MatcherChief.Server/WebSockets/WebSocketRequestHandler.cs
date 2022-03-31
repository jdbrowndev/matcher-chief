using MatcherChief.Server.Matchmaking.Models;
using MatcherChief.Server.Queues;
using MatcherChief.Server.Queues.Models;
using MatcherChief.Shared.Contract;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MatcherChief.Server.WebSockets;

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
			sb.Append(Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
		} while (!receiveResult.EndOfMessage);

		var request = sb.ToString();
		var model = JsonSerializer.Deserialize<MatchRequestModel>(request);

		var queue = _queueManager.GameFormatsToQueues[model.GameFormat];
		var queuedMatchRequest = new QueuedMatchRequestModel
		{
			Id = Guid.NewGuid(),
			WebSocket = webSocket,
			WebSocketCompletionSource = tcs,
			Players = model.Players.Select(x => new Player(x.Id, x.Name)).ToList(),
			Titles = model.GameTitles,
			Modes = model.GameModes,
			QueuedOn = DateTime.Now
		};
		queue.Enqueue(queuedMatchRequest);
	}
}
