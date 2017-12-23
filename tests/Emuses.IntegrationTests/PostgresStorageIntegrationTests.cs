using System;
using System.Globalization;
using Emuses.Exceptions;
using Emuses.Storages;
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
            var sessionCreated = CreateSession(out var entitySelected);

            sessionCreated.GetSessionId().Should().NotBeNullOrEmpty();
            entitySelected.GetSessionId().Should().NotBeNullOrEmpty();
            sessionCreated.GetExpirationDate().Should().BeAfter(DateTime.Now.AddMinutes(55));
            entitySelected.GetExpirationDate().Should().BeAfter(DateTime.Now.AddMinutes(55));
            entitySelected.GetSessionId().Should().Be(sessionCreated.GetSessionId());
            entitySelected.GetExpirationDate().ToString(CultureInfo.InvariantCulture).Should().Be(sessionCreated.GetExpirationDate().ToString(CultureInfo.InvariantCulture));
            entitySelected.GetVersion().Should().Be(sessionCreated.GetVersion());
            entitySelected.GetSessionTimeout().Should().Be(sessionCreated.GetSessionTimeout());
        }

        [Fact]
        public void update_in_db_after_update_session()
        {
            var sessionCreated = CreateSession(out var entitySelected);
            var sessionCreatedVersion = sessionCreated.GetVersion();
            var entityUpdated = UpdateSession(sessionCreated);

            entitySelected.GetVersion().Should().Be(sessionCreatedVersion);
            entityUpdated.GetVersion().Should().NotBe(sessionCreatedVersion);
        }

        [Fact]
        public void delete_in_db_after_close_session()
        {
            var sessionCreated = CreateSession(out var _);

            var exception = Record.Exception(() => sessionCreated.Close());
            exception.Should().BeNull();
        }

        [Fact]
        public void throw_exception_on_delete_in_db_no_exists_session()
        {
            var sessionCreated = CreateSession(out var _);

            sessionCreated.Close();

            var exception = Record.Exception(() => sessionCreated.Close());
            exception.Should().NotBeNull();
            exception.Should().BeAssignableTo<PostgresStorageDeleteException>();
        }

        private static Session UpdateSession(Session session)
        {
            ISessionStorage storage = new PostgresStorage(ConnectionString);
            var entity = storage.GetBySessionId(session.GetSessionId());

            session = new Session(entity.GetSessionId(), entity.GetVersion(), entity.GetSessionTimeout(), entity.GetExpirationDate(), storage);
            session.Update(session.GetSessionId());

            return storage.GetBySessionId(session.GetSessionId());            
        }

        private static Session CreateSession(out Session entitySelected)
        {
            ISessionStorage storage = new PostgresStorage(ConnectionString);
            var session = new Session(storage).Open();

            entitySelected = storage.GetBySessionId(session.GetSessionId());

            return session;
        }
    }
}
