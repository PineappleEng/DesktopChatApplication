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
        public async Task InsertUserChat(UserChat userChat)
        {
            const string query = @"
                INSERT INTO UserChats (UserId, ChatId) 
                VALUES (@UserId, @ChatId);";
            using (var cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@UserId", userChat.UserId);
                cmd.Parameters.AddWithValue("@ChatId", userChat.ChatId);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
