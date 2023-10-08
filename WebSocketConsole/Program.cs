
namespace WebSocketConsole{
    class Program{

        // construction of the websocket
        static System.Net.WebSockets.ClientWebSocket ClientWB = new System.Net.WebSockets.ClientWebSocket();

        // 
        static async Task Main(string[] args){

            // Connection string
            await ClientWB.ConnectAsync(new Uri("ws://localhost:5098/ws"), CancellationToken.None);

            // task runner to await sending async response
            if (ClientWB.State == System.Net.WebSockets.WebSocketState.Open)
            {
                await ClientWB.SendAsync(System.Text.Encoding.ASCII.GetBytes("WebSocket is working and connected through Console app."),
                                         System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
            }

            await Receive();
            Console.ReadKey();

        }

        static async Task Receive(){
            
            bool off = false;
            while (!off)
            {
                if (ClientWB.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    byte[] bt = new byte[1024];
                    var result = await ClientWB.ReceiveAsync(bt, CancellationToken.None);
                    Console.WriteLine(System.Text.Encoding.ASCII.GetString(bt, 0, result.Count));
                }
            }
        }
    }
}
