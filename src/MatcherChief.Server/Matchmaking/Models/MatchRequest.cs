using System;
using System.Collections.Generic;
using MatcherChief.Shared.Enums;

namespace MatcherChief.Server.Matchmaking.Models
{
    public class MatchRequest
    {
        public MatchRequest(Guid id, Player player, IEnumerable<GameTitle> titles, IEnumerable<GameMode> modes, DateTime queuedOn)
        {
            Id = id;
            Player = player;
            Titles = titles;
            Modes = modes;
            QueuedOn = queuedOn;
        }

        public MatchRequest(Player player, IEnumerable<GameTitle> titles, IEnumerable<GameMode> modes, DateTime queuedOn)
        {
            Id = Guid.NewGuid();
            Player = player;
            Titles = titles;
            Modes = modes;
            QueuedOn = queuedOn;
        }

        public Guid Id { get; private set; }
        public Player Player { get; private set; }
        public IEnumerable<GameTitle> Titles { get; private set; }
        public IEnumerable<GameMode> Modes { get; private set; }
        public DateTime QueuedOn { get; private set; }
    }
}