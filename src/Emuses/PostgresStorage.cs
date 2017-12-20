using Emuses.Exceptions;
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
            var session = new Session(0, this);
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT session_id, version, minutes, expiration_date FROM sessions", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sessionIdFromDb = reader.GetString(0);
                            var versionFromDb = reader.GetString(1);
                            var minutesFromDb = reader.GetInt32(2);
                            var expirationDateFromDb = reader.GetDateTime(3);

                            session.Restore(sessionIdFromDb, versionFromDb, expirationDateFromDb, minutesFromDb, this);
                        }
                    }
                }
            }

            return session;
        }

        public Session Create(Session session)
        {
            int result;
            const string query = "INSERT INTO sessions (session_id, version, minutes, expiration_date) VALUES (@session_id, @version, @minutes, @expiration_date)";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("session_id", session.GetSessionId());
                    command.Parameters.AddWithValue("version", session.GetVersion());
                    command.Parameters.AddWithValue("minutes", session.GetMinutes());
                    command.Parameters.AddWithValue("expiration_date", session.GetExpirationDate());
                    result = command.ExecuteNonQuery();
                }
            }

            if (result == 1)
                return session;

            throw new PostgresStorageInsertException();
        }

        public Session Update(Session session)
        {
            int result;
            const string query = "UPDATE sessions SET version = @version, minutes = @minutes, expiration_date = @expiration_date WHERE session_id = @session_id";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("session_id", session.GetSessionId());
                    command.Parameters.AddWithValue("version", session.GetVersion());
                    command.Parameters.AddWithValue("minutes", session.GetMinutes());
                    command.Parameters.AddWithValue("expiration_date", session.GetExpirationDate());
                    result = command.ExecuteNonQuery();
                }
            }

            if (result == 1)
                return session;

            throw new PostgresStorageUpdateException();
        }

        public void Delete(string sessionId)
        {
            int result;
            const string query = "DELETE FROM sessions WHERE session_id = @session_id";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("session_id", sessionId);
                    result = command.ExecuteNonQuery();
                }
            }

            if (result == 1)
                return;

            throw new PostgresStorageDeleteException();
        }
    }
}
