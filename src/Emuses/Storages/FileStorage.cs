using System;
using System.IO;

namespace Emuses.Storages
{
    public class FileStorage : IStorage
    {
        private readonly string _directoryPath;

        private FileStorage() { }

        public FileStorage(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public Session GetBySessionId(string sessionId)
        {
            Session session;
            var fileStream = new FileStream(_directoryPath + sessionId + ".ses", FileMode.Open);
            using (var reader = new StreamReader(fileStream))
            {
                var id = reader.ReadLine().Substring(10);
                var version = reader.ReadLine().Substring(8);
                var minutes = Int32.Parse(reader.ReadLine().Substring(8));
                var expiredDate = DateTime.Parse(reader.ReadLine().Substring(12));

                session = new Session(minutes, this);
                session.Restore(id, version, expiredDate, minutes, this);
            }

            return session;
        }

        public Session Create(Session session)
        {
            using (var file = new StreamWriter(File.Create(_directoryPath + session.GetSessionId() + ".ses")))
            {
                file.WriteLine($"SessionId:{session.GetSessionId()}");
                file.WriteLine($"Version:{session.GetVersion()}");
                file.WriteLine($"Minutes:{session.GetMinutes()}");
                file.WriteLine($"ExpiredDate:{session.GetExpirationDate()}");
            }

            return session;
        }

        public Session Update(Session session)
        {
            using (var file = new StreamWriter(File.OpenWrite(_directoryPath + session.GetSessionId() + ".ses")))
            {
                file.WriteLine($"SessionId:{session.GetSessionId()}");
                file.WriteLine($"Version:{session.GetVersion()}");
                file.WriteLine($"Minutes:{session.GetMinutes()}");
                file.WriteLine($"ExpiredDate:{session.GetExpirationDate()}");
            }

            return session;
        }

        public void Delete(string sessionId)
        {
            File.Delete(_directoryPath + sessionId + ".ses");
        }
    }
}
