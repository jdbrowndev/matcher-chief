using MatcherChief.Shared.Enums;
using System;
using System.Collections.Generic;

namespace MatcherChief.Shared.Contract
{
	public class MatchResponseModel
	{
		public Guid MatchId { get; set; }
		public GameFormat GameFormat { get; set; }
		public GameTitle GameTitle { get; set; }
		public GameMode GameMode { get; set; }
		public IEnumerable<PlayerModel> Players { get; set; }
	}
}