using System;
using Emuses.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Emuses.Tests
{
    public class SessionTests/* : IDisposable*/
    {
        private readonly Mock<IStorage> _storage;

        public SessionTests()
        {
            _storage = new Mock<IStorage>();
        }

        /*public void Dispose() { }*/

        [Fact]
        public void open_new_session()
        {
            ISession session = new Session().Open(30, _storage.Object);
            //_storage.Setup(x => x.Create(session)).Returns(session);

            session.GetSessionId().Should().NotBeNullOrEmpty();
            session.GetExpiredDate().Should().BeAfter(DateTime.Now.AddMinutes(25));
        }

        [Fact]
        public void update_session()
        {
            ISession session = new Session().Open(1, _storage.Object);
            var sessionVersion = session.GetVersion();

            var sessionUpdated = session.Update();

            sessionUpdated.GetExpiredDate().Should().Be(DateTime.Now.AddMinutes(1));
            sessionUpdated.GetVersion().Should().NotBe(sessionVersion);
        }

        [Fact]
        public void throw_exception_on__update_if_session_expired()
        {
            ISession session = new Session().Open(-1, _storage.Object);

            var exception = Record.Exception(() => session.Update());
            exception.Should().NotBeNull();
            exception.Should().BeAssignableTo<SessionExpiredException>();
        }

        [Fact]
        public void close_session()
        {
            ISession session = new Session().Open(1, _storage.Object);

            var sessionClosed = session.Close();

            sessionClosed.GetExpiredDate().Should().BeBefore(DateTime.Now.AddMinutes(1));
            sessionClosed.GetVersion().Should().BeNullOrEmpty();            
        }

        [Fact]
        public void is_valid_session()
        {
            ISession session = new Session().Open(1, _storage.Object);

            session.IsValid().Should().BeTrue();
        }

        [Fact]
        public void restore_session()
        {
            ISession session = new Session();
            const string sessionId = "sessionId";
            const string version = "version";
            var now = DateTime.Now;
            const int minutes = 30;

            session.Restore(sessionId, version, now, minutes, _storage.Object).GetSessionId().Should().Be("sessionId");
            session.Restore(sessionId, version, now, minutes, _storage.Object).GetVersion().Should().Be("version");
            session.Restore(sessionId, version, now, minutes, _storage.Object).GetExpiredDate().Should().Be(now);
            session.Restore(sessionId, version, now, minutes, _storage.Object).GetMinutes().Should().Be(30);            
        }        
    }
}