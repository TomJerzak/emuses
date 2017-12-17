using System.IO;

namespace Emuses
{
    public class FileStorage : IStorage
    {
        private readonly string _directoryPath;
        
        public FileStorage(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public Session GetBySessionId(string sessionId)
        {
            throw new System.NotImplementedException();
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
