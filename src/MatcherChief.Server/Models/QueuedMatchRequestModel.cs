using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using MatcherChief.Core.Models;

namespace MatcherChief.Server.Models
{
    public class QueuedMatchRequestModel
    {
        // TODO: implement
        public WebSocket WebSocket { get; set; }
        public TaskCompletionSource<object> CompletionSource { get; set; }
        public Player Player { get; private set; }
        public IEnumerable<GameTitle> Titles { get; private set; }
        public IEnumerable<GameMode> Modes { get; private set; }
        public DateTime QueuedOn { get; private set; }
    }
}