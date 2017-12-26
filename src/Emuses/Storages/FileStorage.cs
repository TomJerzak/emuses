using System;
using System.Collections.Generic;
using System.IO;

namespace Emuses.Storages
{
    public class FileStorage : ISessionStorage
    {
        private const string SessionId = "SessionId:";
        private const string Version = "Version:";
        private const string SessionTimeout = "SessionTimeout:";
        private const string ExpirationDate = "ExpirationDate:";

        private readonly string _directoryPath;

        private FileStorage()
        {
        }

        public FileStorage(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public IEnumerable<Session> GetAll()
        {
            throw new NotImplementedException();
        }

        public Session GetBySessionId(string sessionId)
        {
            Session session;
            var fileStream = new FileStream(_directoryPath + sessionId + ".ses", FileMode.Open);
            using (var reader = new StreamReader(fileStream))
            {
                var sessionIdFromFile = reader.ReadLine().Substring(SessionId.Length);
                var versionFromFile = reader.ReadLine().Substring(Version.Length);
                var sessionTimeoutFromFile = int.Parse(reader.ReadLine().Substring(SessionTimeout.Length));
                var expirationDateFromFile = DateTime.Parse(reader.ReadLine().Substring(ExpirationDate.Length));

                session = new Session(sessionIdFromFile, versionFromFile, sessionTimeoutFromFile, expirationDateFromFile, this);
            }

            return session;
        }

        public Session Create(Session session)
        {
            using (var file = new StreamWriter(File.Create(_directoryPath + session.GetSessionId() + ".ses")))
            {
                file.WriteLine($"{SessionId}{session.GetSessionId()}");
                file.WriteLine($"{Version}{session.GetVersion()}");
                file.WriteLine($"{SessionTimeout}{session.GetSessionTimeout()}");
                file.WriteLine($"{ExpirationDate}{session.GetExpirationDate()}");
            }

            return session;
        }

        public Session Update(Session session)
        {
            using (var file = new StreamWriter(File.OpenWrite(_directoryPath + session.GetSessionId() + ".ses")))
            {
                file.WriteLine($"SessionId:{session.GetSessionId()}");
                file.WriteLine($"Version:{session.GetVersion()}");
                file.WriteLine($"SessionTimeout:{session.GetSessionTimeout()}");
                file.WriteLine($"ExpirationDate:{session.GetExpirationDate()}");
            }

            return session;
        }

        public void Delete(string sessionId)
        {
            File.Delete(_directoryPath + sessionId + ".ses");
        }
    }
}
