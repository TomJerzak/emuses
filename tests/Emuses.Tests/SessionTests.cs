using System;
using Emuses.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Emuses.Tests
{
    public class SessionTests
    {
        private readonly Mock<ISessionStorage> _storage;

        public SessionTests() => _storage = new Mock<ISessionStorage>();

        [Fact]
        public void open_new_session()
        {
            const int sessionTimeout = 30;
            var session = new Session(sessionTimeout, _storage.Object).Open();

            session.GetSessionId().Should().NotBeNullOrEmpty();
            session.GetExpirationDate().Should().BeAfter(DateTime.Now.AddMinutes(25));
        }

        [Fact]
        public void update_session()
        {
            const int sessionTimeout = 10;
            var session = new Session(sessionTimeout, _storage.Object).Open();
            var sessionVersion = session.GetVersion();
            _storage.Setup(x => x.GetBySessionId(session.GetSessionId())).Returns(session);

            var sessionUpdated = session.Update(session.GetSessionId());

            sessionUpdated.GetVersion().Should().NotBe(sessionVersion);
        }

        [Fact]
        public void throw_exception_on_update_if_session_expired()
        {
            const int sessionTimeout = -1;
            var session = new Session(sessionTimeout, _storage.Object).Open();
            _storage.Setup(x => x.GetBySessionId(session.GetSessionId())).Returns(session);

            var exception = Record.Exception(() => session.Update(session.GetSessionId()));

            exception.Should().NotBeNull();
            exception.Should().BeAssignableTo<SessionExpiredException>();
        }

        [Fact]
        public void close_session()
        {
            const int sessionTimeout = 1;
            var session = new Session(sessionTimeout, _storage.Object).Open();

            var sessionClosed = session.Close();

            sessionClosed.GetExpirationDate().Should().BeBefore(DateTime.Now.AddMinutes(1));
            sessionClosed.GetVersion().Should().BeNullOrEmpty();
        }

        [Fact]
        public void is_valid_session()
        {
            const int sessionTimeout = 1;
            var session = new Session(sessionTimeout, _storage.Object).Open();

            session.IsValid().Should().BeTrue();
        }
    }
}
