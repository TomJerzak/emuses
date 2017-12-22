using System;

namespace Emuses.Example.Models.Home
{
    public class SessionModel
    {
        public Session Session { get; set; }

        public int Minutes { get; set; }

        public int Seconds { get; set; }

        public SessionModel(Session session)
        {
            Session = session;
            SetTimeToCloseSession(session.GetExpirationDate());
        }

        private void SetTimeToCloseSession(DateTime expirationDate)
        {
            var result = (expirationDate - DateTime.Now);
            Minutes = result.Minutes;
            Seconds = result.Seconds;
        }
    }
}
