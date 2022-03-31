using MatcherChief.Server.Matchmaking.Models;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace MatcherChief.Server.Queues.Models;

public class QueuedMatchResponseModel
{
	public Guid RequestId { get; set; }
	public WebSocket WebSocket { get; set; }
	public TaskCompletionSource<object> WebSocketCompletionSource { get; set; }
	public IEnumerable<Player> Players { get; set; }
	public Match Match { get; set; }
}
