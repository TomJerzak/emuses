using System;

namespace Emuses
{
    public interface ISession
    {
        DateTime GetExpiredDate();

        string GetSessionId();

        string GetVersion();

        Session Open(int minutes);

        Session Update();

        Session Close();

        bool IsValid();

        Session Restore(string sessionId, string version, DateTime expiredDateTime, int minutes);
    }
}