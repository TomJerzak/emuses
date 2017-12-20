using System;
using Emuses.Exceptions;

namespace Emuses
{
    public class Session
    {
        private string _sessionId;
        private int _minutes;
        private DateTime _expirationData;
        private string _version;
        private IStorage _storage;

        private Session()
        {
        }

        public Session(int minutes, IStorage storage)
        {
            _minutes = minutes;
            _expirationData = DateTime.Now.AddMinutes(minutes);
            _version = GenerateVersion();
            _storage = storage;
        }

        public Session Open()
        {
            _sessionId = Guid.NewGuid().ToString();
            _expirationData = DateTime.Now.AddMinutes(_minutes);

            _storage.Create(this);
            return this;
        }

        public Session Update()
        {
            if (GetExpirationDate() < DateTime.Now)
                throw new SessionExpiredException();

            _expirationData = DateTime.Now.AddMinutes(_minutes);
            _version = GenerateVersion();

            _storage.Update(this);
            return this;
        }

        public Session Restore(string sessionId, string version, DateTime expiredDateTime, int minutes, IStorage storage)
        {
            _sessionId = sessionId;
            _version = version;
            _expirationData = expiredDateTime;
            _minutes = minutes;
            _storage = storage;

            return this;
        }

        public void SetSessionTimeout(int minutes)
        {
            _minutes = minutes;
        }

        public Session Close()
        {
            _expirationData = DateTime.Now;
            _version = string.Empty;

            _storage.Delete(_sessionId);
            return this;
        }

        public bool IsValid()
        {
            return GetExpirationDate() > DateTime.Now;
        }

        public string GetSessionId()
        {
            return _sessionId;
        }

        public int GetMinutes()
        {
            return _minutes;
        }

        public DateTime GetExpirationDate()
        {
            return _expirationData;
        }

        public string GetVersion()
        {
            return _version;
        }

        // TODO - metoda do usunięcia po zrezygnowaniu z metody Restore. Metoda Update powinna jednocześnie obsługiwać restore ze storage jak i aktualizować sesję.
        public IStorage GetStorage()
        {
            return _storage;
        }

        private static string GenerateVersion()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
