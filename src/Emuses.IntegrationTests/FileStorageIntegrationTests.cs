using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Emuses.IntegrationTests
{
    public class FileStorageIntegrationTests
    {
        private const string DirectoryPath = @"C:\Temp\Emuses";
        private const string SessionFileExtension = ".ses";

        [Fact]
        public void create_file_after_open_new_session()
        {
            var sessionCreated = CreateSession(out var sessionIdLine);

            sessionCreated.GetSessionId().Should().NotBeNullOrEmpty();
            sessionCreated.GetExpiredDate().Should().BeAfter(DateTime.Now.AddMinutes(25));
            sessionIdLine.Should().Be($"SessionId:{sessionCreated.GetSessionId()}");
        }

        [Fact]
        public void restore_saved_session()
        {
            var sessionCreated = CreateSession(out var sessionIdLine);
            IStorage storage = new FileStorage(DirectoryPath + "\\");

            var sessionById = storage.GetBySessionId(sessionCreated.GetSessionId());            

            sessionById.GetVersion().Should().Be(sessionCreated.GetVersion());
        }

        [Fact]
        public void update_file_after_update_session()
        {
            var sessionCreated = CreateSession(out var sessionIdLine);
            IStorage storage = new FileStorage(DirectoryPath + "\\");

            var sessionById = storage.GetBySessionId(sessionCreated.GetSessionId());            
            var sessionUpdated = sessionById.Update();
            sessionUpdated = storage.Update(sessionUpdated);

            sessionIdLine.Should().Be($"SessionId:{sessionCreated.GetSessionId()}");
            sessionUpdated.GetSessionId().Should().Be(sessionCreated.GetSessionId());
            sessionUpdated.GetVersion().Should().NotBe(sessionCreated.GetVersion());
        }

        private static Session CreateSession(out string sessionIdLine)
        {
            IStorage storage = new FileStorage(DirectoryPath + "\\");
            var session = new Session(30, storage).Open();

            Directory.CreateDirectory(DirectoryPath);
            var sessionCreated = storage.Create(session);

            var fileStream = new FileStream(DirectoryPath + "\\" + session.GetSessionId() + SessionFileExtension, FileMode.Open);

            using (var reader = new StreamReader(fileStream))
            {
                sessionIdLine = reader.ReadLine();
            }

            return sessionCreated;
        }
    }
}
