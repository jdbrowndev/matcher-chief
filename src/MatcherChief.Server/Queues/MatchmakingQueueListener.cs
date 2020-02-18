using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MatcherChief.Core;
using MatcherChief.Core.Matchmaking;
using MatcherChief.Core.Models;
using MatcherChief.Server.Models;
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
        private readonly BlockingCollection<QueuedMatchRequestModel> _inQueue;
        private readonly BlockingCollection<QueuedMatchResponseModel> _outQueue;
        private readonly IMatchmakingAlgorithm _matchmakingAlgorithm;
        private readonly ILogger<MatchmakingQueueListener> _logger;
        private readonly Dictionary<Guid, QueuedMatchRequestModel> _requestBuffer;

        public MatchmakingQueueListener(GameFormat format, BlockingCollection<QueuedMatchRequestModel> inQueue,
            BlockingCollection<QueuedMatchResponseModel> outQueue, IMatchmakingAlgorithm matchmakingAlgorithm, ILogger<MatchmakingQueueListener> logger)
        {
            _format = format;
            _inQueue = inQueue;
            _outQueue = outQueue;
            _matchmakingAlgorithm = matchmakingAlgorithm;
            _logger = logger;
            _requestBuffer = new Dictionary<Guid, QueuedMatchRequestModel>();
        }

        public Task Listen(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var queuedRequest = _inQueue.Take();
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
            catch (Exception e)
            {
                _logger.LogError(e, "MatchmakingQueueListener error");
            }
            return Task.CompletedTask;
        }

        private void HandleMatchmakeResult(MatchmakeResult result)
        {
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
                _outQueue.Add(response);
                _requestBuffer.Remove(request.Id);
            }
        }
    }
}