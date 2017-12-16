using System.Linq;
using Emuses.Example.Core.Entities;
using Emuses.Example.Core.Repositories;
using Emuses.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Emuses.Example.Core.Services
{
    public class EmusesSessionService : IEmusesSessionRepository
    {
        private readonly ExampleContext _dbContext;

        public EmusesSessionService(ExampleContext exampleContext)
        {
            _dbContext = exampleContext;
        }

        public IQueryable GetAll()
        {
            return _dbContext.EmusesSessions.AsNoTracking();
        }

        public EmusesSession GetById(long id)
        {
            var emusesSession = _dbContext.EmusesSessions.AsNoTracking().FirstOrDefault(p => p.EmusesSessionId == id);
            if (emusesSession == null)
                throw new SessionNotFoundException();

            return emusesSession;
        }

        public EmusesSession GetBySessionId(string sessionId)
        {
            var emusesSession = _dbContext.EmusesSessions.AsNoTracking().FirstOrDefault(p => p.SessionId.ToLower().Equals(sessionId.ToLower()));
            if (emusesSession == null)
                throw new SessionNotFoundException();

            return emusesSession;
        }

        public EmusesSession Create(EmusesSession item)
        {
            _dbContext.EmusesSessions.Add(item);
            _dbContext.SaveChangesAsync();

            return item;
        }

        public EmusesSession Update(EmusesSession item)
        {
            var entity = GetBySessionId(item.SessionId);
            entity.ExpireDateTime = item.ExpireDateTime;
            entity.Version = item.Version;

            _dbContext.Update(entity);
            _dbContext.SaveChangesAsync();

            return item;
        }

        public EmusesSession Delete(long id)
        {
            var entity = GetById(id);

            _dbContext.Remove(entity);
            _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}