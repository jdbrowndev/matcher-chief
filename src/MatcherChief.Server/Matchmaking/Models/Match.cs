using System;
using System.Collections.Generic;
using MatcherChief.Shared.Enums;

namespace MatcherChief.Server.Matchmaking.Models
{
    public class Match
    {
        public Match(IEnumerable<MatchRequest> requests, GameFormat format, GameTitle title, GameMode mode)
        {
            Id = Guid.NewGuid();
            Requests = requests;
            Format = format;
            Title = title;
            Mode = mode;
        }

        public Guid Id { get; private set; }
        public IEnumerable<MatchRequest> Requests { get; private set; }
        public GameFormat Format { get; private set; }
        public GameTitle Title { get; private set; }
        public GameMode Mode { get; private set; }
    }
}