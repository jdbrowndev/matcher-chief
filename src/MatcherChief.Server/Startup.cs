using System;
using MatcherChief.Server.Matchmaking;
using MatcherChief.Server.Matchmaking.PreferenceScore;
using MatcherChief.Server.Queues;
using MatcherChief.Server.Queues.Auditing;
using MatcherChief.Server.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MatcherChief.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMatchmakingAlgorithm, PreferenceScoreMatchmakingAlgorithm>();
            services.AddTransient<IPreferenceScoreCalculator, PreferenceScoreCalculator>();
            services.AddTransient<IMatchmakingQueueListenerFactory, MatchmakingQueueListenerFactory>();
            services.AddTransient<IAuditLoggerFactory, AuditLoggerFactory>();
            services.AddTransient<IWebSocketRequestHandler, WebSocketRequestHandler>();
            services.AddTransient<IWebSocketResponseHandler, WebSocketResponseHandler>();

            services.AddSingleton<IQueueManager, QueueManager>();
            services.AddSingleton<IOutboundQueueListener, OutboundQueueListener>();
            services.AddHostedService<MatchmakingHostedService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var webSocketOptions = new WebSocketOptions() 
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 1024 * 4
            };

            app.UseWebSockets(webSocketOptions);
            app.UseMiddleware<WebSocketHandlerMiddleware>();
        }
    }
}
