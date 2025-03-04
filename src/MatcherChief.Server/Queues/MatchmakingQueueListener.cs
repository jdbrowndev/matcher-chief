using MatcherChief.Server.Matchmaking;
using MatcherChief.Server.Matchmaking.Models;
using MatcherChief.Server.Queues.Models;
using MatcherChief.Shared.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MatcherChief.Server.Queues;

public interface IMatchmakingQueueListener
{
	GameFormat Format { get; }
	int BufferCount { get; }
	IEnumerable<DateTime> BufferQueuedOnTimestamps { get; }
	long MatchCount { get; }
	Task Listen(CancellationToken token);
}

public class MatchmakingQueueListener : IMatchmakingQueueListener
{
	private readonly GameFormat _format;
	private readonly string _formatName;
	private readonly AsyncConcurrentQueue<QueuedMatchRequestModel> _inQueue;
	private readonly AsyncConcurrentQueue<QueuedMatchResponseModel> _outQueue;
	private readonly IMatchmakingAlgorithm _matchmakingAlgorithm;
	private readonly ILogger<MatchmakingQueueListener> _logger;
	private readonly ConcurrentDictionary<Guid, QueuedMatchRequestModel> _requestBuffer;
	private long _matchCount;

	public GameFormat Format { get { return _format; } }

	public int BufferCount { get { return _requestBuffer.Count; } }

	public IEnumerable<DateTime> BufferQueuedOnTimestamps
	{
		get { return _requestBuffer.Values.Select(x => x.QueuedOn).ToList(); }
	}

	public long MatchCount { get { return _matchCount; } }

	public MatchmakingQueueListener(GameFormat format, AsyncConcurrentQueue<QueuedMatchRequestModel> inQueue,
		AsyncConcurrentQueue<QueuedMatchResponseModel> outQueue, IMatchmakingAlgorithm matchmakingAlgorithm, ILogger<MatchmakingQueueListener> logger)
	{
		_format = format;
		_formatName = Enum.GetName(typeof(GameFormat), _format);
		_inQueue = inQueue;
		_outQueue = outQueue;
		_matchmakingAlgorithm = matchmakingAlgorithm;
		_logger = logger;
		_requestBuffer = new ConcurrentDictionary<Guid, QueuedMatchRequestModel>();
	}

	public async Task Listen(CancellationToken token)
	{
		_logger.LogInformation("MatchmakingQueueListener ({FormatName}) listening...", _formatName);
		while (!token.IsCancellationRequested)
		{
			try
			{
				var queuedRequest = await _inQueue.DequeueAsync(token);
				_requestBuffer.AddOrUpdate(queuedRequest.Id, queuedRequest, (k, v) => queuedRequest);

				var requests = _requestBuffer.Values
					.Select(x => new MatchRequest(x.Id, x.Players, x.Titles, x.Modes, x.QueuedOn))
					.ToList();

				var result = _matchmakingAlgorithm.Matchmake(_format, requests);
				HandleMatchmakeResult(result);
			}
			catch (OperationCanceledException)
			{
				_logger.LogInformation("MatchmakingQueueListener ({FormatName}) shutting down...", _formatName);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "MatchmakingQueueListener ({FormatName}) error", _formatName);
			}
		}
	}

	private void HandleMatchmakeResult(MatchmakeResult result)
	{
		var responses = new List<QueuedMatchResponseModel>();

		foreach (var match in result.Matches)
			foreach (var request in match.Requests)
			{
				var queuedRequest = _requestBuffer[request.Id];
				var response = new QueuedMatchResponseModel
				{
					RequestId = queuedRequest.Id,
					WebSocket = queuedRequest.WebSocket,
					WebSocketCompletionSource = queuedRequest.WebSocketCompletionSource,
					Players = queuedRequest.Players,
					Match = match
				};
				responses.Add(response);
				_requestBuffer.Remove(request.Id, out _);
			}

		_outQueue.EnqueueRange(responses);
		_matchCount += result.Matches.Count();
	}
}
