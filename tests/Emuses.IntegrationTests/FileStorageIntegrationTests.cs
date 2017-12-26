using System;
using System.IO;
using System.Linq;
using Emuses.Storages;
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
            sessionCreated.GetExpirationDate().Should().BeAfter(DateTime.Now.AddMinutes(25));
            sessionIdLine.Should().Be($"SessionId:{sessionCreated.GetSessionId()}");
        }

        [Fact]
        public void restore_saved_session()
        {
            var sessionCreated = CreateSession(out var _);
            ISessionStorage storage = new FileStorage(DirectoryPath + "\\");

            var sessionById = storage.GetBySessionId(sessionCreated.GetSessionId());

            sessionById.GetVersion().Should().Be(sessionCreated.GetVersion());
        }

        [Fact]
        public void update_file_after_update_session()
        {
            var sessionCreated = CreateSession(out var sessionIdLine);
            ISessionStorage storage = new FileStorage(DirectoryPath + "\\");

            var sessionById = storage.GetBySessionId(sessionCreated.GetSessionId());
            var sessionUpdated = sessionById.Update(sessionCreated.GetSessionId());

            sessionIdLine.Should().Be($"SessionId:{sessionCreated.GetSessionId()}");
            sessionUpdated.GetSessionId().Should().Be(sessionCreated.GetSessionId());
            sessionUpdated.GetVersion().Should().NotBe(sessionCreated.GetVersion());
        }

        [Fact]
        public void delete_file_after_close_session()
        {
            var sessionCreated = CreateSession(out var _);

            sessionCreated.Close();

            File.Exists($"{DirectoryPath}\\{sessionCreated.GetSessionId()}.ses").Should().BeFalse();
        }

        [Fact]
        public void get_all_sessions_from_db()
        {
            CreateSession(out var _);
            CreateSession(out var _);

            ISessionStorage storage = new FileStorage(DirectoryPath + "\\");

            storage.GetAll().Count().Should().BeGreaterThan(1);
        }

        private static Session CreateSession(out string sessionIdLine)
        {
            Directory.CreateDirectory(DirectoryPath);
            ISessionStorage storage = new FileStorage(DirectoryPath + "\\");
            var session = new Session(30, storage).Open();

            var fileStream = new FileStream(DirectoryPath + "\\" + session.GetSessionId() + SessionFileExtension, FileMode.Open);
            using (var reader = new StreamReader(fileStream))
                sessionIdLine = reader.ReadLine();

            return session;
        }
    }
}
