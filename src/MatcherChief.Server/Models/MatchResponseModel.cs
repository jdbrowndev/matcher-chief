using System;
using System.Collections.Generic;

namespace MatcherChief.Server.Models
{
    public class MatchResponseModel
    {
        public Guid MatchId { get; set; }
        public int GameFormat { get; set; }
        public int GameTitle { get; set; }
        public int GameMode { get; set; }
        public IEnumerable<MatchResponsePlayerModel> Players { get; set; }
    }

    public class MatchResponsePlayerModel
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
    }
}