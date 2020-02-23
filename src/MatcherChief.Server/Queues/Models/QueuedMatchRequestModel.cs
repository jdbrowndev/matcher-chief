using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using MatcherChief.Server.Matchmaking.Models;
using MatcherChief.Shared.Enums;

namespace MatcherChief.Server.Queues.Models
{
    public class QueuedMatchRequestModel
    {
        public Guid Id { get; set; }
        public WebSocket WebSocket { get; set; }
        public TaskCompletionSource<object> WebSocketCompletionSource { get; set; }
        public Player Player { get; set; }
        public IEnumerable<GameTitle> Titles { get; set; }
        public IEnumerable<GameMode> Modes { get; set; }
        public DateTime QueuedOn { get; set; }
    }
}