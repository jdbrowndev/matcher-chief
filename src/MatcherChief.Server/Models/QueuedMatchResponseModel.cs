using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using MatcherChief.Core.Models;

namespace MatcherChief.Server.Models
{
    public class QueuedMatchResponseModel
    {
        public Guid RequestId { get; set; }
        public WebSocket WebSocket { get; set; }
        public TaskCompletionSource<object> WebSocketCompletionSource { get; set; }
        public Player Player { get; set; }
        public Match Match { get; set; }
    }
}