using MatcherChief.Server.Queues.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Text.Json;

namespace MatcherChief.Server.Queues.Auditing;

public interface IAuditLogger : IDisposable
{
	void LogResponse(QueuedMatchResponseModel response);
}

public class AuditLogger : IAuditLogger
{
	private readonly ILogger<AuditLogger> _logger;
	private readonly MemoryCache _matchDeduplicationCache;
	private readonly bool _isDebug = Log.IsEnabled(LogEventLevel.Debug);

	public AuditLogger(ILogger<AuditLogger> logger)
	{
		_logger = logger;
		_matchDeduplicationCache = new MemoryCache(new MemoryCacheOptions());
	}

	public void LogResponse(QueuedMatchResponseModel response)
	{
		if (!_isDebug)
			return;

		var match = response.Match;
		if (_matchDeduplicationCache.TryGetValue(match.Id, out var obj))
			return;

		var json = JsonSerializer.Serialize(response.Match);
		_logger.LogDebug("Successful match: {Match}", json);

		var options = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) };
		_matchDeduplicationCache.Set(match.Id, true, options);
	}

	public void Dispose()
	{
		_matchDeduplicationCache.Dispose();
	}
}