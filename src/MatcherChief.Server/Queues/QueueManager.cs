using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MatcherChief.Core.Models;
using MatcherChief.Server.Models;

namespace MatcherChief.Server.Queues
{
    public interface IQueueManager
    {
        Dictionary<GameFormat, BlockingCollection<QueuedMatchRequestModel>> GameFormatsToQueues { get; }
        BlockingCollection<QueuedMatchResponseModel> OutboundQueue { get; }
    }

    public class QueueManager : IQueueManager
    {
        public QueueManager()
        {
            GameFormatsToQueues = new Dictionary<GameFormat, BlockingCollection<QueuedMatchRequestModel>>();

            foreach (GameFormat format in Enum.GetValues(typeof(GameFormat)))
            {
                var collection = new BlockingCollection<QueuedMatchRequestModel>(new ConcurrentQueue<QueuedMatchRequestModel>());
                GameFormatsToQueues.Add(format, collection);
            }

            OutboundQueue = new BlockingCollection<QueuedMatchResponseModel>(new ConcurrentQueue<QueuedMatchResponseModel>());
        }

        public Dictionary<GameFormat, BlockingCollection<QueuedMatchRequestModel>> GameFormatsToQueues { get; private set; }
        public BlockingCollection<QueuedMatchResponseModel> OutboundQueue { get; private set; }
    }
}