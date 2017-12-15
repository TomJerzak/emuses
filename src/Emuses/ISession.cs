using System;

namespace Emuses
{
    public interface ISession
    {
        DateTime GetExpiredDate();

        string GetSessionId();

        string GetVersion();

        Session Update();

        Session Close();

        bool IsValid();
    }
}