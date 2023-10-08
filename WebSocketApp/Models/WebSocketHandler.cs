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

            await next();
        }
    }
}
