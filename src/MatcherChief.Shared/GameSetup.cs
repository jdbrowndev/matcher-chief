using System.Collections.Generic;
using MatcherChief.Shared.Enums;

namespace MatcherChief.Shared
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
            { GameFormat.Firefight, new [] { GameMode.FirefightArcade, GameMode.FirefightLimited } }
        };

        public static readonly Dictionary<GameFormat, int> GameFormatsToPlayersRequired = new Dictionary<GameFormat, int>
        {
            { GameFormat.OneVersusOne, 2 },
            { GameFormat.TwoVersusTwo, 4 },
            { GameFormat.FourVersusFour, 8 },
            { GameFormat.EightPlayerFFA, 8 },
            { GameFormat.TwelvePlayerFFA, 12 },
            { GameFormat.EightVersusEight, 16 },
            { GameFormat.Firefight, 4 }
        };
    }
}