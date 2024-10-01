using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;

public class WebSocketManager
{
    private readonly ConcurrentDictionary<Guid, WebSocket> _sockets = new();

    public async Task HandleWebSocketConnectionAsync(WebSocket webSocket)
    {
        var socketId = Guid.NewGuid();
        _sockets.TryAdd(socketId, webSocket);

        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                _sockets.TryRemove(socketId, out _);
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
            }
        }
    }

    public async Task BroadcastMessageAsync(Message message)
    {
        var messageString = System.Text.Json.JsonSerializer.Serialize(message);
        var messageBytes = Encoding.UTF8.GetBytes(messageString);

        foreach (var socket in _sockets.Values)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
