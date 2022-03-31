using MatcherChief.Shared.Enums;
using System.Collections.Generic;

namespace MatcherChief.Shared.Contract
{
	public class MatchRequestModel
	{
		public IEnumerable<PlayerModel> Players { get; set; }
		public GameFormat GameFormat { get; set; }
		public IEnumerable<GameTitle> GameTitles { get; set; }
		public IEnumerable<GameMode> GameModes { get; set; }
	}
}