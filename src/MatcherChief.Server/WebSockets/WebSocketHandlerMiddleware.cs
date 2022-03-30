using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MatcherChief.Server.WebSockets;

public class WebSocketHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebSocketRequestHandler _requestHandler;

    public WebSocketHandlerMiddleware(RequestDelegate next, IWebSocketRequestHandler requestHandler)
    {
        _next = next;
        _requestHandler = requestHandler;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/ws")
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                var socketFinishedTcs = new TaskCompletionSource<object>(); 

                await _requestHandler.Handle(webSocket, socketFinishedTcs);
                await socketFinishedTcs.Task;
                
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Response sent", CancellationToken.None);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }
        else
        {
            await _next(context);
        }
    }
}
