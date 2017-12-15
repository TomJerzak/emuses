using System.Linq;
using Emuses.Example.Core.Entities;
using Emuses.Example.Core.Repositories;
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
            return _dbContext.EmusesSessions.AsNoTracking().FirstOrDefault(p => p.EmusesSessionId == id);
        }

        public EmusesSession GetBySessionId(string sessionId)
        {
            return _dbContext.EmusesSessions.AsNoTracking().FirstOrDefault(p => p.SessionId.ToLower().Equals(sessionId.ToLower()));
        }

        public EmusesSession Create(EmusesSession item)
        {
            _dbContext.EmusesSessions.Add(item);
            _dbContext.SaveChangesAsync();

            return item;
        }

        public EmusesSession Update(EmusesSession item)
        {
            var entity = GetById(item.EmusesSessionId);
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