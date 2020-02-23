using System;
using System.Linq;
using MatcherChief.Server.Api.Models;
using MatcherChief.Server.HostedService;
using MatcherChief.Server.Queues;
using MatcherChief.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatcherChief.Server.Api.Controllers
{
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
            var queues = _queueManager.GameFormatsToQueues
                .Select(kvp => new SystemQueueStatusModel
                {
                    Name = Enum.GetName(typeof(GameFormat), kvp.Key),
                    Count = kvp.Value.Count,
                    Buffered = _hostedService.MatchmakingQueueListeners.FirstOrDefault(x => x.Format == kvp.Key)?.BufferCount ?? 0
                })
                .Concat(new[]
                {
                    new SystemQueueStatusModel
                    {
                        Name = "Outbound",
                        Count = _queueManager.OutboundQueue.Count,
                        Buffered = 0
                    }
                })
                .ToList();

            var model = new SystemStatusModel
            {
                Queues = queues
            };

            return model;
        }
    }
}