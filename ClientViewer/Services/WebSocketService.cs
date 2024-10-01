using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ClientViewer.Models;

namespace ClientViewer.Services;

public class WebSocketService
{
    private readonly string _serverUri = "ws://localhost:5000/ws/messages";
    public List<Message> Messages { get; private set; } = new List<Message>();

    public WebSocketService()
    {
        Task.Run(ConnectWebSocketAsync);
    }

    private async Task ConnectWebSocketAsync()
    {
        using var client = new ClientWebSocket();

        try
        {
            await client.ConnectAsync(new Uri(_serverUri), CancellationToken.None);
            Console.WriteLine("WebSocket connected!");

            var buffer = new byte[1024 * 4];
            while (client.State == WebSocketState.Open)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var receivedMessage = JsonSerializer.Deserialize<Message>(message);

                    if (receivedMessage != null)
                    {
                        Messages.Add(receivedMessage);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket connection error: {ex.Message}");
        }
    }
}
