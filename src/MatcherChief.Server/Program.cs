using MatcherChief.Server.HostedService;
using MatcherChief.Server.Matchmaking;
using MatcherChief.Server.Matchmaking.PreferenceScore;
using MatcherChief.Server.Queues;
using MatcherChief.Server.Queues.Auditing;
using MatcherChief.Server.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.CreateBootstrapLogger();

try
{
	var builder = WebApplication.CreateBuilder(args);
	builder.WebHost.UseKestrel();
	builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
	builder.Configuration.AddEnvironmentVariables();
	builder.Host.UseDefaultServiceProvider((context, options) =>
	{
		options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
	});
	builder.Host.UseSerilog((context, config) => config.WriteTo.Console().ReadFrom.Configuration(context.Configuration));

	var services = builder.Services;
	services.AddTransient<IMatchmakingAlgorithm, PreferenceScoreMatchmakingAlgorithm>();
	services.AddTransient<IPreferenceScoreCalculator, PreferenceScoreCalculator>();
	services.AddTransient<IMatchmakingQueueListenerFactory, MatchmakingQueueListenerFactory>();
	services.AddTransient<IAuditLogger, AuditLogger>();
	services.AddTransient<IWebSocketRequestHandler, WebSocketRequestHandler>();
	services.AddTransient<IWebSocketResponseHandler, WebSocketResponseHandler>();

	services.AddHostedService<MatchmakingHostedService>();
	services.AddTransient<IHostedServiceAccessor<MatchmakingHostedService>, HostedServiceAccessor<MatchmakingHostedService>>();

	services.AddSingleton<IQueueManager, QueueManager>();
	services.AddSingleton<IOutboundQueueListener, OutboundQueueListener>();

	services.AddControllers();

	var app = builder.Build();
	var webSocketOptions = new WebSocketOptions()
	{
		KeepAliveInterval = TimeSpan.FromSeconds(120)
	};

	app.UseWebSockets(webSocketOptions);
	app.UseMiddleware<WebSocketHandlerMiddleware>();

	app.UseRouting();
	app.UseEndpoints(endpoints =>
	{
		endpoints.MapControllerRoute("default", "/api/{controller}/{action}/{id?}");
	});

	app.Run();
}
catch (Exception exception)
{
	Log.Fatal(exception, "Unhandled fatal exception");
}
finally
{
	Log.CloseAndFlush();
}