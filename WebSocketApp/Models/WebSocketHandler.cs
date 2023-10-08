using System;
using System.Net.WebSockets;
using WebSocketApp;

namespace WebSocketApp
{
    public class WebSocketHandler
    {
        private readonly WebSocketModel _webSocketModel;

        public WebSocketHandler(WebSocketModel webSocketModel)
        {
            _webSocketModel = webSocketModel;
        }

        public async Task Handle(HttpContext context, Func<Task> next)
        {
            if(context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                _webSocketModel.WebSocket = webSocket;

                var buffer = new byte[1024 * 4];
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                        break;
                    }

                    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            else
            {
                // Only invoke next middleware when it's not a WebSocket request to allow the pipeline to continue
                await next();
            }
        }
    }
}
