using System.Collections.Generic;
using MatcherChief.Core.Models;

namespace MatcherChief.Core
{
    public static class GameSetup
    {
        public static readonly Dictionary<GameFormat, IEnumerable<GameMode>> GameFormatsToModes = new Dictionary<GameFormat, IEnumerable<GameMode>>
        {
            { GameFormat.OneVersusOne, new [] { GameMode.Slayer } },
            { GameFormat.TwoVersusTwo, new [] { GameMode.Slayer, GameMode.FlagAndBomb, GameMode.ZoneControl, GameMode.AssetDenial, GameMode.ActionSack } },
            { GameFormat.FourVersusFour, new [] { GameMode.Slayer, GameMode.FlagAndBomb, GameMode.ZoneControl, GameMode.AssetDenial, GameMode.ActionSack, GameMode.Swat, GameMode.Snipers } },
            { GameFormat.EightPlayerFFA, new [] { GameMode.Slayer, GameMode.ZoneControl, GameMode.AssetDenial, GameMode.ActionSack, GameMode.Swat, GameMode.Snipers } },
            { GameFormat.TwelvePlayerFFA, new [] { GameMode.Infection } },
            { GameFormat.EightVersusEight, new [] { GameMode.Slayer, GameMode.FlagAndBomb, GameMode.ZoneControl, GameMode.AssetDenial, GameMode.ActionSack, GameMode.Snipers, GameMode.Heavies } },
            { GameFormat.Firefight, new [] { GameMode.FirefightArcade, GameMode.FirefightLimited } },
        };
    }
}