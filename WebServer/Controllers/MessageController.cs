using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

[Route("api/messages")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageRepository _messageRepository;
    // private readonly WebSocketManager _webSocketManager;

    public MessageController(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    // public MessageController(IMessageRepository messageRepository, WebSocketManager webSocketManager)
    // {
    //     _messageRepository = messageRepository;
    //     _webSocketManager = webSocketManager;
    // }

    [HttpPost]
    public IActionResult PostMessage([FromBody] Message message)
    {
        if (string.IsNullOrWhiteSpace(message.Text) || message.Text.Length > 128)
        {
            return BadRequest("Message text must be provided and cannot exceed 128 characters.");
        }

        message.Timestamp = DateTime.UtcNow;

        _messageRepository.AddMessage(message);
        // await _webSocketManager.BroadcastMessageAsync(message);

        return Ok();
    }

    [HttpGet]
    public IActionResult GetMessages([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        DateTime rangeStart = startDate ?? DateTime.MinValue;
        DateTime rangeEnd = endDate ?? DateTime.UtcNow;

        var messages = _messageRepository.GetMessages(rangeStart, rangeEnd);

        if (messages == null || messages.Count == 0)
        {
            return NotFound("No messages found.");
        }

        return Ok(messages);
    }
}
