using System;
using System.Collections.Generic;
using MatcherChief.Shared.Enums;

namespace MatcherChief.Shared.Contract
{
    public class MatchRequestModel
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public GameFormat GameFormat { get; set; }
        public IEnumerable<GameTitle> GameTitles { get; set; }
        public IEnumerable<GameMode> GameModes { get; set; }
    }
}