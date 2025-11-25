using Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Server.Database
{
    internal partial class ChatDatabase : IDisposable
    {
        public async Task InsertUser(User user)
        {
            const string query = @"
                INSERT INTO Users (Name, HashedPassword)
                VALUES (@Name, @HashedPassword);";
            using (var cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@HashedPassword", user.HashedPassword);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public List<User> GetAllUsers()
        {
            const string query = "SELECT * FROM Users;";
            List<User> users = new List<User>();
            using (var cmd = new SQLiteCommand(query, _connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        HashedPassword = reader.GetString(reader.GetOrdinal("HashedPassword"))
                    });
                }
            }
            return users;
        }

        public User GetUserByName(string name)
        {
            const string query = @"
                SELECT * 
                FROM Users 
                WHERE Name = @Name;";
            using (var cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@Name", name);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            HashedPassword = reader.GetString(reader.GetOrdinal("HashedPassword"))
                        };
                    }
                }
            }
            return null; // No user found
        }

        public List<User> GetChatMembers(int chatId)
        {
            const string query = @"
                SELECT Id, Name
                FROM Users
                JOIN UserChats
                ON Users.Id = UserChats.UserId
                AND ChatId = @ChatId;";
            List<User> users = new List<User>();
            using (var cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@ChatId", chatId);
                using (var reader = cmd.ExecuteReader())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name"))
                    });
                }
            }
            return users;
        }
    }
}
