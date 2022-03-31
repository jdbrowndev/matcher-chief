using MatcherChief.Shared.Enums;
using System;
using System.Collections.Generic;

namespace MatcherChief.Server.Matchmaking.Models;

public class MatchRequest
{
	public MatchRequest(Guid id, IEnumerable<Player> players, IEnumerable<GameTitle> titles, IEnumerable<GameMode> modes, DateTime queuedOn)
	{
		Id = id;
		Players = players;
		Titles = titles;
		Modes = modes;
		QueuedOn = queuedOn;
	}

	public MatchRequest(IEnumerable<Player> players, IEnumerable<GameTitle> titles, IEnumerable<GameMode> modes, DateTime queuedOn)
	{
		Id = Guid.NewGuid();
		Players = players;
		Titles = titles;
		Modes = modes;
		QueuedOn = queuedOn;
	}

	public Guid Id { get; private set; }
	public IEnumerable<Player> Players { get; private set; }
	public IEnumerable<GameTitle> Titles { get; private set; }
	public IEnumerable<GameMode> Modes { get; private set; }
	public DateTime QueuedOn { get; private set; }
}
