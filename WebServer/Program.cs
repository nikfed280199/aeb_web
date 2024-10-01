using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddSingleton(new WebSocketManager()); 

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<IMessageRepository>(new MessageRepository(connectionString, builder.Services.BuildServiceProvider().GetRequiredService<ILogger<MessageRepository>>()));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// var webSocketOptions = new WebSocketOptions
// {
//     KeepAliveInterval = TimeSpan.FromSeconds(120)
// };
// app.UseWebSockets(webSocketOptions);

// app.Map("/ws/messages", async context =>
// {
//     if (context.WebSockets.IsWebSocketRequest)
//     {
//         var webSocket = await context.WebSockets.AcceptWebSocketAsync();
//         var webSocketManager = context.RequestServices.GetRequiredService<WebSocketManager>();
//         await webSocketManager.HandleWebSocketConnectionAsync(webSocket);
//     }
//     else
//     {
//         context.Response.StatusCode = StatusCodes.Status400BadRequest;
//     }
// });

app.Run();