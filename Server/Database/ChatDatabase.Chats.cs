using Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Server.Database
{
    internal partial class ChatDatabase : IDisposable
    {
        public async Task InsertChat(Chat chat)
        {
            // Insert the chat and return last_insert_rowid()
            const string query = @"
                INSERT INTO Chats (Name, AdminId) 
                VALUES (@Name, @AdminId);
                SELECT last_insert_rowid();";

            using (var cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@Name", chat.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@AdminId", chat.AdminId);

                var result = await cmd.ExecuteScalarAsync().ConfigureAwait(false);

                // ExecuteScalar for last_insert_rowid returns a long
                if (result is long id)
                {
                    chat.Id = Convert.ToInt32(id);
                }
                else if (result != null)
                {
                    chat.Id = Convert.ToInt32(result);
                }
                else
                {
                    throw new InvalidOperationException("Failed to retrieve inserted chat id.");
                }
            }
        }

        public List<Chat> GetUserChats(int userId)
        {
            const string query = @"
                SELECT c.Id, c.Name, c.AdminId
                FROM Chats c
                INNER JOIN UserChats uc ON c.Id = uc.ChatId
                WHERE uc.UserId = @UserId;";

            var chats = new List<Chat>();
            using (var cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        chats.Add(new Chat
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            AdminId = reader.GetInt32(reader.GetOrdinal("AdminId"))
                        });
                    }
                }
            }

            return chats;
        }

        public Chat GetChatById(int chatId)
        {
            // Use the actual column name (Id) in the WHERE clause
            const string query = "SELECT Id, Name, AdminId FROM Chats WHERE Id = @ChatId;";
            using (var cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@ChatId", chatId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Chat
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            AdminId = reader.GetInt32(reader.GetOrdinal("AdminId"))
                        };
                    }
                }
            }
            return null; // No chat found
        }
    }
}
