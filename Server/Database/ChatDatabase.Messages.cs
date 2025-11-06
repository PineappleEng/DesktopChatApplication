using Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

        public List<KeyValuePair<string, Message>> GetChatMessages(Chat chat)
        {
            const string query = @"
                SELECT Messages.Id, ChatId, SenderId, Users.Name as SenderName, Timestamp, Content
                FROM Messages
                JOIN Users ON Messages.SenderId = Users.Id
                WHERE ChatId = @ChatId
                ORDER BY Timestamp ASC;";

            List<KeyValuePair<string, Message>> messages = new List<KeyValuePair<string, Message>>();

            using (var cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@ChatId", chat.Id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string senderName = reader.GetString(reader.GetOrdinal("SenderName"));
                        Message message = new Message
                        {
                            SenderId = reader.GetInt32(reader.GetOrdinal("SenderId")),
                            Timestamp = reader.GetString(reader.GetOrdinal("Timestamp")),
                            Content = reader.GetString(reader.GetOrdinal("Content"))
                        };

                        messages.Add(new KeyValuePair<string, Message>(senderName, message));
                    }
                }
            }

            return messages;
        }

    }
}
