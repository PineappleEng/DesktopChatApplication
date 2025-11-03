using System;
using System.Data;
using System.Data.SQLite;

namespace Server.Database
{
    internal partial class ChatDatabase : IDisposable
    {
        private readonly SQLiteConnection _connection;

        public ChatDatabase()
        {
            string connectionString
                = $"Data Source=../../Database/ChatApp.db;Version=3;";
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
        }

        public void Dispose()
        {
            if (_connection != null )
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
                _connection.Dispose();
            }
        }
    }
}
