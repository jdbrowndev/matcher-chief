using System.Collections.Generic;

namespace MatcherChief.Core.Models
{
    public class Match
    {
        public Match(GameFormat format, GameTitle title, GameMode mode, IEnumerable<Player> players)
        {
            Format = format;
            Title = title;
            Mode = mode;
            Players = players;
        }

        public GameFormat Format { get; private set; }
        public GameTitle Title { get; private set; }
        public GameMode Mode { get; private set; }
        public IEnumerable<Player> Players { get; private set; }
    }
}