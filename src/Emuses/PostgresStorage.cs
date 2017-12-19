using Npgsql;

namespace Emuses
{
    public class PostgresStorage : IStorage
    {
        private readonly string _connectionString;

        private PostgresStorage()
        {
        }

        public PostgresStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Session GetBySessionId(string sessionId)
        {
            throw new System.NotImplementedException();
        }

        public Session Create(Session session)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO sessions (session_id) VALUES (@session_id)";
                    command.Parameters.AddWithValue("session_id", session.GetSessionId());
                    command.ExecuteNonQuery();
                }
            }

            throw new System.NotImplementedException();
        }

        public Session Update(Session session)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string sessionId)
        {
            throw new System.NotImplementedException();
        }
    }
}

/*
 var connString = "Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase";

using (var conn = new NpgsqlConnection(connString))
{
    conn.Open();

    // Insert some data
    using (var cmd = new NpgsqlCommand())
    {
        cmd.Connection = conn;
        cmd.CommandText = "INSERT INTO data (some_field) VALUES (@p)";
        cmd.Parameters.AddWithValue("p", "Hello world");
        cmd.ExecuteNonQuery();
    }

    // Retrieve all rows
    using (var cmd = new NpgsqlCommand("SELECT some_field FROM data", conn))
    using (var reader = cmd.ExecuteReader())
        while (reader.Read())
            Console.WriteLine(reader.GetString(0));
}
     */
