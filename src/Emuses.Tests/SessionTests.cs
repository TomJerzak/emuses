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
            var session = new Session(30, _storage.Object).Open();
            //_storage.Setup(x => x.Create(session)).Returns(session);

            session.GetSessionId().Should().NotBeNullOrEmpty();
            session.GetExpirationDate().Should().BeAfter(DateTime.Now.AddMinutes(25));
        }

        [Fact]
        public void update_session()
        {
            var session = new Session(1, _storage.Object).Open();
            // var initExpiredDate = session.GetExpirationDate();
            var sessionVersion = session.GetVersion();

            var sessionUpdated = session.Update();

            // initExpiredDate.Should().NotBe(sessionUpdated.GetExpirationDate());
            sessionUpdated.GetVersion().Should().NotBe(sessionVersion);
        }

        [Fact]
        public void throw_exception_on_update_if_session_expired()
        {
            var session = new Session(-1, _storage.Object).Open();

            var exception = Record.Exception(() => session.Update());
            exception.Should().NotBeNull();
            exception.Should().BeAssignableTo<SessionExpiredException>();
        }

        [Fact]
        public void close_session()
        {
            var session = new Session(1, _storage.Object).Open();

            var sessionClosed = session.Close();

            sessionClosed.GetExpirationDate().Should().BeBefore(DateTime.Now.AddMinutes(1));
            sessionClosed.GetVersion().Should().BeNullOrEmpty();            
        }

        [Fact]
        public void is_valid_session()
        {
            var session = new Session(1, _storage.Object).Open();

            session.IsValid().Should().BeTrue();
        }

        [Fact]
        public void restore_session()
        {
            const string sessionId = "sessionId";
            const string version = "version";
            var now = DateTime.Now;
            const int minutes = 30;
            var session = new Session(minutes, _storage.Object);            

            session.Restore(sessionId, version, now, minutes, _storage.Object).GetSessionId().Should().Be("sessionId");
            session.Restore(sessionId, version, now, minutes, _storage.Object).GetVersion().Should().Be("version");
            session.Restore(sessionId, version, now, minutes, _storage.Object).GetExpirationDate().Should().Be(now);
            session.Restore(sessionId, version, now, minutes, _storage.Object).GetMinutes().Should().Be(30);            
        }        
    }
}