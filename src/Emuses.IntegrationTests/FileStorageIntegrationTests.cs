using System;
using FluentAssertions;
using Xunit;

namespace Emuses.IntegrationTests
{
    public class FileStorageIntegrationTests
    {
        [Fact]
        public void create_file_after_open_new_session()
        {
            IStorage storage = new FileStorage();
            var session = new Session().Open(30, storage);

            var sessionCreated = storage.Create(session);

            sessionCreated.GetSessionId().Should().NotBeNullOrEmpty();
            sessionCreated.GetExpiredDate().Should().BeAfter(DateTime.Now.AddMinutes(25));
        }
    }
}
