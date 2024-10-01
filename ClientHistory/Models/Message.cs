namespace ClientHistory.Models;

public class Message
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public DateTime Timestamp { get; set; }
    public required int SequenceNumber { get; set; }
}