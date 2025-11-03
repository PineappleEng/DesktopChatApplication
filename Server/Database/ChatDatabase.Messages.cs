using Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Database
{
    internal partial class ChatDatabase : IDisposable
    {
        public async Task InsertMessage(Message message)
        {
            const string query = @"
                INSERT INTO Messages (ChatId, SenderId, Content)
                VALUES (@ChatId, @SenderId, @Content);";
            using (var cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@ChatId", message.ChatId);
                cmd.Parameters.AddWithValue("@SenderId", message.SenderId);
                cmd.Parameters.AddWithValue("@Content", message.Content);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public List<Message> GetChatMessages(int chatId)
        {
            const string query = @"
                SELECT * 
                FROM Messages
                WHERE ChatId = @ChatId
                ORDER BY Timestamp ASC;";
            List<Message> messages = new List<Message>();
            using (var cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@ChatId", chatId);
                using (var reader = cmd.ExecuteReader())
                {
                    messages.Add(new Message
                    {
                        SenderId = reader.GetInt32(reader.GetOrdinal("SenderId")),
                        Timestamp = reader.GetString(reader.GetOrdinal("Timestamp")),
                        Content = reader.GetString(reader.GetOrdinal("Content"))
                    });
                }
            }
            return messages;
        }
    }
}
