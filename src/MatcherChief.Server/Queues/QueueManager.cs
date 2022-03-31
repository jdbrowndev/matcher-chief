using MatcherChief.Server.Queues.Models;
using MatcherChief.Shared.Enums;
using System;
using System.Collections.Generic;

namespace MatcherChief.Server.Queues;

public interface IQueueManager
{
	Dictionary<GameFormat, AsyncConcurrentQueue<QueuedMatchRequestModel>> GameFormatsToQueues { get; }
	AsyncConcurrentQueue<QueuedMatchResponseModel> OutboundQueue { get; }
}

public class QueueManager : IQueueManager
{
	public QueueManager()
	{
		GameFormatsToQueues = new Dictionary<GameFormat, AsyncConcurrentQueue<QueuedMatchRequestModel>>();

		foreach (GameFormat format in Enum.GetValues(typeof(GameFormat)))
		{
			var collection = new AsyncConcurrentQueue<QueuedMatchRequestModel>();
			GameFormatsToQueues.Add(format, collection);
		}

		OutboundQueue = new AsyncConcurrentQueue<QueuedMatchResponseModel>();
	}

	public Dictionary<GameFormat, AsyncConcurrentQueue<QueuedMatchRequestModel>> GameFormatsToQueues { get; private set; }
	public AsyncConcurrentQueue<QueuedMatchResponseModel> OutboundQueue { get; private set; }
}
