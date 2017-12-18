using System;
using Emuses.Exceptions;

namespace Emuses
{
    public class Session : ISession
    {
        private string _sessionId;
        private int _minutes;
        private DateTime _expireDateTime;
        private string _version;
        private IStorage _storage;

        private Session()
        {
        }

        public Session(int minutes, IStorage storage)
        {
            _minutes = minutes;
            _expireDateTime = DateTime.Now.AddMinutes(minutes);
            _version = GenerateVersion();
            _storage = storage;
        }

        public Session Open()
        {
            _sessionId = Guid.NewGuid().ToString();
            _expireDateTime = DateTime.Now.AddMinutes(_minutes);

            _storage.Create(this);
            return this;
        }

        public Session Update()
        {
            if (GetExpiredDate() < DateTime.Now)
                throw new SessionExpiredException();

            _expireDateTime = DateTime.Now.AddMinutes(_minutes);
            _version = GenerateVersion();

            _storage.Update(this);
            return this;
        }

        public Session Restore(string sessionId, string version, DateTime expiredDateTime, int minutes, IStorage storage)
        {
            _sessionId = sessionId;
            _version = version;
            _expireDateTime = expiredDateTime;
            _minutes = minutes;
            _storage = storage;

            return this;
        }

        public Session Close()
        {
            _expireDateTime = DateTime.Now;
            _version = string.Empty;

            _storage.Delete(_sessionId);
            return this;
        }

        public bool IsValid()
        {
            return GetExpiredDate() > DateTime.Now;
        }

        public string GetSessionId()
        {
            return _sessionId;
        }

        public int GetMinutes()
        {
            return _minutes;
        }

        public DateTime GetExpiredDate()
        {
            return _expireDateTime;
        }

        public string GetVersion()
        {
            return _version;
        }

        private static string GenerateVersion()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
