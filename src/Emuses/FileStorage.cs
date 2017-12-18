using System;
using System.IO;
using static System.Int32;

namespace Emuses
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
                var minutes = Parse(reader.ReadLine().Substring(8));
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
                file.WriteLine($"ExpiredDate:{session.GetExpiredDate()}");
            }

            return session;
        }

        public Session Update(Session session)
        {
            throw new System.NotImplementedException();
        }

        public Session Delete(string sessionId)
        {
            throw new System.NotImplementedException();
        }
    }
}
