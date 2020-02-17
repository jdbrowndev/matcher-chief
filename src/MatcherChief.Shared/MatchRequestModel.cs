using System;
using System.Collections.Generic;

namespace MatcherChief.Shared
{
    public class MatchRequestModel
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int GameFormat { get; set; }
        public IEnumerable<int> GameTitles { get; set; }
        public IEnumerable<int> GameModes { get; set; }
    }
}