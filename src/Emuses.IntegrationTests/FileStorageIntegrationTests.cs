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
            IStorage storage = new FileStorage(DirectoryPath + "\\");
            var session = new Session().Open(30, storage);

            Directory.CreateDirectory(DirectoryPath);
            var sessionCreated = storage.Create(session);
            
            var fileStream = new FileStream(DirectoryPath + "\\" + session.GetSessionId() + SessionFileExtension, FileMode.Open);
            string sessionIdLine;
            using (var reader = new StreamReader(fileStream))
            {
                sessionIdLine = reader.ReadLine();
            }

            sessionCreated.GetSessionId().Should().NotBeNullOrEmpty();
            sessionCreated.GetExpiredDate().Should().BeAfter(DateTime.Now.AddMinutes(25));
            sessionIdLine.Should().Be($"SessionId:{session.GetSessionId()}");
        }
    }
}
