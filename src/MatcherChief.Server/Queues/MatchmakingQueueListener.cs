using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Server.Matchmaking;
using MatcherChief.Server.Matchmaking.Models;
using MatcherChief.Server.Models;
using MatcherChief.Shared;
using MatcherChief.Shared.Enums;
using Microsoft.Extensions.Logging;

namespace MatcherChief.Server.Queues
{
    public interface IMatchmakingQueueListener
    {
        Task Listen(CancellationToken token);
    }

    public class MatchmakingQueueListener : IMatchmakingQueueListener
    {
        private readonly GameFormat _format;
        private readonly AsyncConcurrentQueue<QueuedMatchRequestModel> _inQueue;
        private readonly AsyncConcurrentQueue<QueuedMatchResponseModel> _outQueue;
        private readonly IMatchmakingAlgorithm _matchmakingAlgorithm;
        private readonly ILogger<MatchmakingQueueListener> _logger;
        private readonly Dictionary<Guid, QueuedMatchRequestModel> _requestBuffer;

        public MatchmakingQueueListener(GameFormat format, AsyncConcurrentQueue<QueuedMatchRequestModel> inQueue,
            AsyncConcurrentQueue<QueuedMatchResponseModel> outQueue, IMatchmakingAlgorithm matchmakingAlgorithm, ILogger<MatchmakingQueueListener> logger)
        {
            _format = format;
            _inQueue = inQueue;
            _outQueue = outQueue;
            _matchmakingAlgorithm = matchmakingAlgorithm;
            _logger = logger;
            _requestBuffer = new Dictionary<Guid, QueuedMatchRequestModel>();
        }

        public async Task Listen(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var queuedRequest = await _inQueue.DequeueAsync(token);
                    _requestBuffer.Add(queuedRequest.Id, queuedRequest);

                    var playersRequired = GameSetup.GameFormatsToPlayersRequired[_format];

                    if (_requestBuffer.Count >= playersRequired)
                    {
                        var requests = _requestBuffer.Values
                            .Select(x => new MatchRequest(x.Id, queuedRequest.Player, queuedRequest.Titles, queuedRequest.Modes, queuedRequest.QueuedOn))
                            .ToList();

                        var result = _matchmakingAlgorithm.Matchmake(_format, requests);

                        HandleMatchmakeResult(result);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                var name = Enum.GetName(typeof(GameFormat), _format);
                _logger.LogInformation($"MatchmakingQueueListener for {name} queue shutting down...");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "MatchmakingQueueListener error");
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
                    Player = queuedRequest.Player,
                    Match = match
                };
                responses.Add(response);
                _requestBuffer.Remove(request.Id);
            }

            _outQueue.EnqueueRange(responses);
        }
    }
}