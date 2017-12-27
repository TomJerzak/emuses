using System.Collections.Generic;

namespace Emuses
{
    public interface ISessionStorage
    {
        IEnumerable<Session> GetAll();

        Session GetBySessionId(string sessionId);

        Session Create(Session session);

        Session Update(Session session);

        void Delete(string sessionId);
    }
}
