using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace Emuses.Tests
{
    public class SessionManagerTests
    {
        private readonly Mock<ISessionStorage> _storage;

        public SessionManagerTests()
        {
            _storage = new Mock<ISessionStorage>();
        }

        [Fact]
        public void return_all_sessions()
        {
            var manger = new SessionManager(_storage.Object);
            var sessions = new List<Session>
            {
                new Session("1", "1", 1, DateTime.Now, _storage.Object),
                new Session("2", "1", 1, DateTime.Now, _storage.Object)
            };
            _storage.Setup(p => p.GetAll()).Returns(sessions);

            manger.GetAll().Should().HaveCount(2);
        }
    }
}