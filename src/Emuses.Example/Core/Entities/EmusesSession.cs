using System;

namespace Emuses.Example.Core.Entities
{
    public class EmusesSession
    {
        public long EmusesSessionId { get; set; }

        public string SessionId { get; set; }

        public int Minutes { get; set; }

        public DateTime ExpireDateTime { get; set; }

        public string Version { get; set; }

        public EmusesSession()
        {
        }

        public EmusesSession(Session session)
        {            
            SessionId = session.GetSessionId();
            Minutes = session.GetMinutes();
            ExpireDateTime = session.GetExpiredDate();
            Version = session.GetVersion();
        }
    }
}
