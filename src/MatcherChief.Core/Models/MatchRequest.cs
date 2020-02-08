using System;
using System.Collections.Generic;

namespace MatcherChief.Core.Models
{
    public class MatchRequest
    {
        public Player Player { get; set; }
        public IEnumerable<GameTitle> Titles { get; set; }
        public IEnumerable<GameMode> Modes { get; set; }
        public DateTime QueuedOn { get; set; }
    }
}