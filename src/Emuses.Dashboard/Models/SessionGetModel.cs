using System;

namespace Emuses.Dashboard.Models
{
    public class SessionGetModel
    {
        public string SessionId { get; set; }

        public string Version { get; set; }

        public int SessionTimeout { get; set; }

        public DateTime ExpirationDate { get; set; }

        public SessionGetModel()
        {
        }

        public SessionGetModel(string sessionId, string version, int sessionTimeout, DateTime expirationDate)
        {
            SessionId = sessionId;
            Version = version;
            SessionTimeout = sessionTimeout;
            ExpirationDate = expirationDate;
        }
    }
}
