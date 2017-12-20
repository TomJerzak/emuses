using System;
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
            var sessionCreated = CreateSession(out var entity);

            sessionCreated.GetSessionId().Should().NotBeNullOrEmpty();
            sessionCreated.GetExpirationDate().Should().BeAfter(DateTime.Now.AddMinutes(25));
            entity.GetSessionId().Should().Be(sessionCreated.GetSessionId());
            // entity.GetExpirationDate().Should().Be(sessionCreated.GetExpirationDate());
            entity.GetVersion().Should().Be(sessionCreated.GetVersion());
            entity.GetMinutes().Should().Be(sessionCreated.GetMinutes());
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
            IStorage storage = new PostgresStorage(ConnectionString);
            var entity = storage.GetBySessionId(session.GetSessionId());

            session.Restore(entity.GetSessionId(), entity.GetVersion(), entity.GetExpirationDate(), entity.GetMinutes(), storage);
            var result = session.Update();

            return storage.GetBySessionId(session.GetSessionId());            
        }

        private static Session CreateSession(out Session entitySelected)
        {
            IStorage storage = new PostgresStorage(ConnectionString);
            var session = new Session(30, storage).Open();

            entitySelected = storage.GetBySessionId(session.GetSessionId());

            return session;
        }
    }
}
