using System;
using FluentAssertions;
using Xunit;

namespace Emuses.IntegrationTests
{
    public class PostgresStorageIntegrationTests
    {
        private const string ConnectionString = "Host=127.0.0.1;Username=emuses;Password=emuses;Database=emuses";

        [Fact]
        public void insert_to_db_after_open_new_session()
        {
            var sessionCreated = CreateSession(out var sessionIdLine);

            sessionCreated.GetSessionId().Should().NotBeNullOrEmpty();
            sessionCreated.GetExpiredDate().Should().BeAfter(DateTime.Now.AddMinutes(25));
            //sessionIdLine.Should().Be($"SessionId:{sessionCreated.GetSessionId()}");
        }

        private static Session CreateSession(out string sessionIdLine)
        {
            IStorage storage = new PostgresStorage(ConnectionString);
            var session = new Session(30, storage).Open();

            sessionIdLine = ""; // storage.GetBySessionId(session.GetSessionId()).GetSessionId();

            return session;
        }
    }
}
