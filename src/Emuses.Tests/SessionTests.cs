using System;
using FluentAssertions;
using Xunit;

namespace Emuses.Tests
{
    public class SessionTests
    {
        [Fact]
        public void open_new_session()
        {
            ISession session = new Session().Open(30);

            session.GetSessionId().Should().NotBeNullOrEmpty();
            session.GetExpiredDate().Should().BeAfter(DateTime.Now.AddMinutes(25));
        }

        [Fact]
        public void update_session()
        {
            ISession session = new Session().Open(1);
            var sessionVersion = session.GetVersion();

            var sessionUpdated = session.Update();

            sessionUpdated.GetExpiredDate().Should().Be(DateTime.Now.AddMinutes(1));
            sessionUpdated.GetVersion().Should().NotBe(sessionVersion);
        }

        [Fact]
        public void close_session()
        {
            ISession session = new Session().Open(1);

            var sessionClosed = session.Close();

            sessionClosed.GetExpiredDate().Should().Be(DateTime.Now);
            sessionClosed.GetVersion().Should().BeNullOrEmpty();            
        }

        [Fact]
        public void is_valid_session()
        {
            ISession session = new Session().Open(1);

            session.IsValid().Should().BeTrue();
        }
    }
}