using System.Collections.Generic;

namespace MatcherChief.Server.Api.Models
{
    public class SystemStatusModel
    {
        public IEnumerable<SystemQueueStatusModel> Queues { get; set; }
    }

    public class SystemQueueStatusModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public int Buffered { get; set; }
    }
}