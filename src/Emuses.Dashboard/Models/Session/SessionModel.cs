using System;

namespace Emuses.Dashboard.Models.Session
{
    public class SessionModel
    {
        public string SessionId { get; set; }

        public string Version { get; set; }

        public int SessionTimeout { get; set; }

        public DateTime ExpirationDate { get; set; }

        public SessionModel() { }
        
        public SessionModel(string sessionId, string version, int sessionTimeout, DateTime expirationDate)
        {
            SessionId = sessionId;
            Version = version;
            SessionTimeout = sessionTimeout;
            ExpirationDate = expirationDate;
        }
    }
}
