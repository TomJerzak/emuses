using System;

namespace Emuses
{
    public class Session : ISession
    {
        private readonly string _sessionId;
        private readonly int _minutes;
        private DateTime _expireDateTime;
        private string _version;

        public Session()
        {
        }

        public Session(string sessionId, int minutes)
        {
            _sessionId = sessionId;
            _minutes = minutes;
            _expireDateTime = DateTime.Now.AddMinutes(minutes);
            _version = GenerateVersion();
        }
        
        public Session Open(int minutes)
        {
            return new Session(Guid.NewGuid().ToString(), minutes);
        }

        public Session Update()
        {
            _expireDateTime = DateTime.Now.AddMinutes(_minutes);
            _version = GenerateVersion();
            return this;
        }

        public Session Close()
        {
            _expireDateTime = DateTime.Now;
            _version = string.Empty;

            return this;
        }

        public string GetSessionId()
        {
            return _sessionId;
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