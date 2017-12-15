using System.Linq;
using Emuses.Example.Core.Entities;

namespace Emuses.Example.Core.Repositories
{
    public interface IEmusesSessionRepository
    {
        IQueryable GetAll();

        EmusesSession GetById(long id);

        EmusesSession Create(EmusesSession item);

        EmusesSession Update(EmusesSession item);

        EmusesSession Delete(long id);
    }
}
