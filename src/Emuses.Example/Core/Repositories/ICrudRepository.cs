using System.Linq;

namespace Emuses.Example.Core.Repositories
{
    public interface ICrudRepository<T>
    {
        IQueryable GetAll();

        T GetById(int id);

        T Create(T item);

        T Update(T item);

        T Delete(int id);
    }
}
