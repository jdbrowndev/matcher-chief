using MatcherChief.Shared.Enums;
using System.Collections.Generic;

namespace MatcherChief.Shared
{
	public static class GameSetup
	{
		public static readonly Dictionary<GameFormat, IEnumerable<GameTitle>> GameFormatsToTitles = new Dictionary<GameFormat, IEnumerable<GameTitle>>
		{
			{ GameFormat.OneVersusOne, new [] { GameTitle.HaloReach, GameTitle.HaloCE, GameTitle.Halo2, GameTitle.Halo2Anniversary, GameTitle.Halo3, GameTitle.Halo4 } },
			{ GameFormat.TwoVersusTwo, new [] { GameTitle.HaloReach, GameTitle.HaloCE, GameTitle.Halo2, GameTitle.Halo2Anniversary, GameTitle.Halo3, GameTitle.Halo4 } },
			{ GameFormat.FourVersusFour, new [] { GameTitle.HaloReach, GameTitle.HaloCE, GameTitle.Halo2, GameTitle.Halo2Anniversary, GameTitle.Halo3, GameTitle.Halo4 } },
			{ GameFormat.EightPlayerFFA, new [] { GameTitle.HaloReach, GameTitle.HaloCE, GameTitle.Halo2, GameTitle.Halo2Anniversary, GameTitle.Halo3, GameTitle.Halo4 } },
			{ GameFormat.TwelvePlayerFFA, new [] { GameTitle.HaloReach, GameTitle.Halo2Anniversary, GameTitle.Halo3, GameTitle.Halo4 } },
			{ GameFormat.EightVersusEight, new [] { GameTitle.HaloReach, GameTitle.HaloCE, GameTitle.Halo2, GameTitle.Halo2Anniversary, GameTitle.Halo3, GameTitle.Halo4 } },
			{ GameFormat.TwoPlayerFirefight, new [] { GameTitle.HaloReach, GameTitle.Halo3ODST } },
			{ GameFormat.FourPlayerFirefight, new [] { GameTitle.HaloReach, GameTitle.Halo3ODST } }
		};

		public static readonly Dictionary<GameFormat, IEnumerable<GameMode>> GameFormatsToModes = new Dictionary<GameFormat, IEnumerable<GameMode>>
		{
			{ GameFormat.OneVersusOne, new [] { GameMode.Slayer } },
			{ GameFormat.TwoVersusTwo, new [] { GameMode.Slayer, GameMode.FlagAndBomb, GameMode.ZoneControl, GameMode.AssetDenial, GameMode.ActionSack } },
			{ GameFormat.FourVersusFour, new [] { GameMode.Slayer, GameMode.FlagAndBomb, GameMode.ZoneControl, GameMode.AssetDenial, GameMode.ActionSack, GameMode.Swat, GameMode.Snipers } },
			{ GameFormat.EightPlayerFFA, new [] { GameMode.Slayer, GameMode.ZoneControl, GameMode.AssetDenial, GameMode.ActionSack, GameMode.Swat, GameMode.Snipers } },
			{ GameFormat.TwelvePlayerFFA, new [] { GameMode.Infection } },
			{ GameFormat.EightVersusEight, new [] { GameMode.Slayer, GameMode.FlagAndBomb, GameMode.ZoneControl, GameMode.AssetDenial, GameMode.ActionSack, GameMode.Snipers, GameMode.Heavies } },
			{ GameFormat.TwoPlayerFirefight, new [] { GameMode.FirefightScoreAttack, GameMode.FirefightBlam, GameMode.FirefightSniper } },
			{ GameFormat.FourPlayerFirefight, new [] { GameMode.FirefightHeroic, GameMode.FirefightArcade, GameMode.FirefightBlam, GameMode.FirefightSniper } }
		};

		public static readonly Dictionary<GameFormat, int> GameFormatsToPlayersRequired = new Dictionary<GameFormat, int>
		{
			{ GameFormat.OneVersusOne, 2 },
			{ GameFormat.TwoVersusTwo, 4 },
			{ GameFormat.FourVersusFour, 8 },
			{ GameFormat.EightPlayerFFA, 8 },
			{ GameFormat.TwelvePlayerFFA, 12 },
			{ GameFormat.EightVersusEight, 16 },
			{ GameFormat.TwoPlayerFirefight, 2 },
			{ GameFormat.FourPlayerFirefight, 4 }
		};
	}
}