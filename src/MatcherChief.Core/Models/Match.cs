using System.Collections.Generic;

namespace MatcherChief.Core.Models
{
    public class Match
    {
        public GameFormat Format { get; set; }
        public GameTitle Title { get; set; }
        public GameMode Mode { get; set; }
        public IEnumerable<Player> Players { get; set; }
    }
}