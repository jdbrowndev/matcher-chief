using System;
using System.Collections.Generic;
using MatcherChief.Shared.Enums;

namespace MatcherChief.Shared.Contract
{
    public class MatchResponseModel
    {
        public Guid MatchId { get; set; }
        public GameFormat GameFormat { get; set; }
        public GameTitle GameTitle { get; set; }
        public GameMode GameMode { get; set; }
        public IEnumerable<MatchResponsePlayerModel> Players { get; set; }
    }

    public class MatchResponsePlayerModel
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
    }
}