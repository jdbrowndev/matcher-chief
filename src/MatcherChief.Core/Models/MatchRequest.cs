using System;
using System.Collections.Generic;

namespace MatcherChief.Core.Models
{
    public class MatchRequest
    {
        public MatchRequest(Player player, IEnumerable<GameTitle> titles, IEnumerable<GameMode> modes, DateTime queuedOn)
        {
            Player = player;
            Titles = titles;
            Modes = modes;
            QueuedOn = queuedOn;
        }

        public Player Player { get; private set; }
        public IEnumerable<GameTitle> Titles { get; private set; }
        public IEnumerable<GameMode> Modes { get; private set; }
        public DateTime QueuedOn { get; private set; }
    }
}