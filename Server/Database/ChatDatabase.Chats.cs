using Shared.Models;
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
        public async Task InsertChat(Chat chat)
        {
            const string query = @"
                INSERT INTO Chats (Name, AdminId) 
                VALUES (@Name, @AdminId);";
            using (var cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@Name", chat.Name);
                cmd.Parameters.AddWithValue("@AdminId", chat.AdminId);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public List<Chat> GetUserChats(int userId)
        {
            const string query = @"
                SELECT Id, Name, AdminId
                FROM Chats
                JOIN UserChats
                ON Chats.Id = UserChats.ChatId
                AND UserChats.UserId = @UserId;";
            List<Chat> chats = new List<Chat>();
            using (var cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    chats.Add(new Chat
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        AdminId = reader.GetInt32(reader.GetOrdinal("AdminId"))
                    });
                }
            }
            return chats;
        }

        public Chat GetChatById(int chatId)
        {
            const string query = "SELECT * FROM Chats WHERE ChatId = @ChatId;";
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
