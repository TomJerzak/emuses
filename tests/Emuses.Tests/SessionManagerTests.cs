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

            manger.GetAll().Should().HaveCount(2);
        }
    }
}