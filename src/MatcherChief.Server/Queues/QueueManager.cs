using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MatcherChief.Core.Models;

namespace MatcherChief.Server.Queues
{
    public interface IQueueManager
    {
        Dictionary<GameFormat, BlockingCollection<MatchRequest>> GameFormatsToQueues { get; }
        BlockingCollection<MatchRequest> OutboundQueue { get; }
    }

    public class QueueManager : IQueueManager
    {
        public QueueManager()
        {
            GameFormatsToQueues = new Dictionary<GameFormat, BlockingCollection<MatchRequest>>();

            foreach (GameFormat format in Enum.GetValues(typeof(GameFormat)))
            {
                var collection = new BlockingCollection<MatchRequest>(new ConcurrentQueue<MatchRequest>());
                GameFormatsToQueues.Add(format, collection);
            }

            OutboundQueue = new BlockingCollection<MatchRequest>(new ConcurrentQueue<MatchRequest>());
        }

        public Dictionary<GameFormat, BlockingCollection<MatchRequest>> GameFormatsToQueues { get; private set; }
        public BlockingCollection<MatchRequest> OutboundQueue { get; private set; }
    }
}