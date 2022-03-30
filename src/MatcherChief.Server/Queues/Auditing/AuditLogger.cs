using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MatcherChief.Server.Queues.Models;
using Microsoft.Extensions.Caching.Memory;

namespace MatcherChief.Server.Queues.Auditing;

public interface IAuditLogger : IDisposable
{
	Task LogResponse(QueuedMatchResponseModel response);
}

public class AuditLogger : IAuditLogger
{
	private readonly FileStream _fs;
	private readonly StreamWriter _writer;
	private readonly MemoryCache _matchDeduplicationCache;
	private bool _needsComma;

	public AuditLogger(string path)
	{
		_fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
		_writer = new StreamWriter(_fs) { AutoFlush = true };
		_writer.Write("[");

		_matchDeduplicationCache = new MemoryCache(new MemoryCacheOptions());
	}

	public async Task LogResponse(QueuedMatchResponseModel response)
	{
		var match = response.Match;

		if (_matchDeduplicationCache.TryGetValue(match.Id, out var obj))
			return;

		var json = JsonSerializer.Serialize(response.Match);

		if (_needsComma)
			json = ",\n\t" + json;
		else
			json = "\n\t" + json;

		await _writer.WriteAsync(json);
		_needsComma = true;

		var options = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) };
		_matchDeduplicationCache.Set(match.Id, true, options);
	}

	public void Dispose()
	{
		_writer.Write("\n]");
		_writer.Dispose();
		_fs.Dispose();
	}
}

public class NoopAuditLogger : IAuditLogger
{
	public Task LogResponse(QueuedMatchResponseModel response)
	{
		return Task.CompletedTask;
	}

	public void Dispose()
	{
	}
}
