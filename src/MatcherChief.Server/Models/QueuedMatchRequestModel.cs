using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using MatcherChief.Core.Models;

namespace MatcherChief.Server.Models
{
    public class QueuedMatchRequestModel
    {
        public WebSocket WebSocket { get; set; }
        public TaskCompletionSource<object> CompletionSource { get; set; }
        public Player Player { get; set; }
        public IEnumerable<GameTitle> Titles { get; set; }
        public IEnumerable<GameMode> Modes { get; set; }
        public DateTime QueuedOn { get; set; }
    }
}