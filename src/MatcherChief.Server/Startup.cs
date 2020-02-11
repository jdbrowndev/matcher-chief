using MatcherChief.Server.Queues;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MatcherChief.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMatchmakingQueueListenerFactory, MatchmakingQueueListenerFactory>();
            
            services.AddSingleton<IQueueManager, QueueManager>();
            services.AddSingleton<IOutboundQueueListener, OutboundQueueListener>();
            services.AddHostedService<MatchmakingHostedService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
            });
        }
    }
}
