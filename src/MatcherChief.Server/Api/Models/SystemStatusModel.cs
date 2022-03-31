using System.Collections.Generic;

namespace MatcherChief.Server.Api.Models;

public class SystemStatusModel
{
	public bool AllBackgroundTasksRunning { get; set; }
	public long TotalMatches { get; set; }
	public IEnumerable<SystemQueueStatusModel> Queues { get; set; }
}

public class SystemQueueStatusModel
{
	public string Name { get; set; }
	public int Count { get; set; }
	public int BufferedCount { get; set; }
	public SystemQueueTimeAggregatesModel QueueTimes { get; set; }
	public long MatchCount { get; set; }
}

public class SystemQueueTimeAggregatesModel
{
	public string Min { get; set; }
	public string Max { get; set; }
	public string Average { get; set; }
}
