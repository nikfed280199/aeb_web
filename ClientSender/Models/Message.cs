namespace ClientSender.Models;

public class Message
{
    public required string Text { get; set; }
    public required int SequenceNumber { get; set; }
}