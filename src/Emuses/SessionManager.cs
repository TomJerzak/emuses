using System.Collections.Generic;

namespace Emuses
{
    public class SessionManager
    {
        private readonly ISessionStorage _sessionStorage;

        public SessionManager(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public List<Session> GetAll()
        {
            return _sessionStorage.GetAll();
        }
    }
}