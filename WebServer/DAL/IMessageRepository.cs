public interface IMessageRepository
{
    void AddMessage(Message message);
    List<Message> GetMessages(DateTime startDate, DateTime endDate);
}