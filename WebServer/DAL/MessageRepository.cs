using Npgsql;
using Microsoft.Extensions.Logging;

public class MessageRepository : IMessageRepository
{
    private readonly string _connectionString;
    private readonly ILogger<MessageRepository> _logger;

    public MessageRepository(string connectionString, ILogger<MessageRepository> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        
        var createTableQuery = @"
            CREATE TABLE IF NOT EXISTS messages (
                id SERIAL PRIMARY KEY,
                text VARCHAR(128) NOT NULL,
                timestamp TIMESTAMP NOT NULL,
                sequence_number INT NOT NULL
            );
        ";

        using var command = new NpgsqlCommand(createTableQuery, connection);
        command.ExecuteNonQuery();
    }

    public void AddMessage(Message message)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var cmd = new NpgsqlCommand("INSERT INTO messages (text, timestamp, sequence_number) VALUES (@text, @timestamp, @sequence_number)", connection);
        cmd.Parameters.AddWithValue("text", message.Text);
        cmd.Parameters.AddWithValue("timestamp", message.Timestamp);
        cmd.Parameters.AddWithValue("sequence_number", message.SequenceNumber);
        
        cmd.ExecuteNonQuery();
        _logger.LogInformation("Message added: {text} at {timestamp}", message.Text, message.Timestamp);
    }

    public List<Message> GetMessages(DateTime startDate, DateTime endDate)
    {
        var messages = new List<Message>();

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var cmd = new NpgsqlCommand("SELECT id, text, timestamp, sequence_number FROM messages WHERE timestamp BETWEEN @startDate AND @endDate", connection);
        cmd.Parameters.AddWithValue("startDate", startDate);
        cmd.Parameters.AddWithValue("endDate", endDate);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            messages.Add(new Message
            {
                Id = reader.GetInt32(0),
                Text = reader.GetString(1),
                Timestamp = reader.GetDateTime(2),
                SequenceNumber = reader.GetInt32(3)
            });
        }

        return messages;
    }
}
