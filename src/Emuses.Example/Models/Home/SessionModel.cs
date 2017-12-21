using System;

namespace Emuses.Example.Models.Home
{
    public class SessionModel
    {
        public Session Session { get; set; }

        public string TimeToCloseSession { get; set; }

        public SessionModel(Session session)
        {
            Session = session;

            TimeToCloseSession = GetTimeToCloseSession(session.GetExpirationDate());
        }

        private static string GetTimeToCloseSession(DateTime expirationDate)
        {
            var result = (expirationDate - DateTime.Now);

            return result.Hours.ToString("00") + ":" + result.Minutes.ToString("00") + ":" + result.Seconds.ToString("00");
        }
    }
}
