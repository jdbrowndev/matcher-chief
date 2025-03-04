using MatcherChief.Server.Api.Models;
using MatcherChief.Server.HostedService;
using MatcherChief.Server.Queues;
using MatcherChief.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MatcherChief.Server.Api.Controllers;

public class SystemController : Controller
{
	private readonly MatchmakingHostedService _hostedService;
	private readonly IQueueManager _queueManager;

	public SystemController(IHostedServiceAccessor<MatchmakingHostedService> hostedServiceAccessor, IQueueManager queueManager)
	{
		_hostedService = hostedServiceAccessor.Service;
		_queueManager = queueManager;
	}

	[HttpGet]
	public SystemStatusModel Status()
	{
		var now = DateTime.Now;

		var queues = _queueManager.GameFormatsToQueues
			.Select(kvp =>
			{
				var listener = _hostedService.MatchmakingQueueListeners.Single(x => x.Format == kvp.Key);
				var queuedTimes = listener.BufferQueuedOnTimestamps.Select(x => now - x).Select(x => x.TotalSeconds).ToList();
				return new SystemQueueStatusModel
				{
					Name = Enum.GetName(typeof(GameFormat), kvp.Key),
					Count = kvp.Value.Count,
					BufferedCount = listener.BufferCount,
					QueueTimes = new SystemQueueTimeAggregatesModel
					{
						Min = queuedTimes.Any() ? $"{queuedTimes.Min():N0}s" : "0s",
						Max = queuedTimes.Any() ? $"{queuedTimes.Max():N0}s" : "0s",
						Average = queuedTimes.Any() ? $"{queuedTimes.Average():N0}s" : "0s"
					},
					MatchCount = listener.MatchCount
				};
			})
			.Concat([
				new SystemQueueStatusModel
				{
					Name = "Outbound",
					Count = _queueManager.OutboundQueue.Count,
					BufferedCount = 0,
					QueueTimes = new SystemQueueTimeAggregatesModel
					{
						Min = "0s",
						Max = "0s",
						Average = "0s"
					},
					MatchCount = 0
				}
			])
			.ToList();

		var model = new SystemStatusModel
		{
			AllBackgroundTasksRunning = _hostedService.BackgroundTasks.All(x => !x.IsCompleted),
			TotalMatches = queues.Sum(x => x.MatchCount),
			Queues = queues
		};

		return model;
	}
}
