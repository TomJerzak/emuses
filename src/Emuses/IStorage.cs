namespace Emuses
{
    public interface IStorage
    {
        Session GetBySessionId(string sessionId);

        Session Create(Session session);

        Session Update(Session session);

        Session Delete(string sessionId);
    }
}
