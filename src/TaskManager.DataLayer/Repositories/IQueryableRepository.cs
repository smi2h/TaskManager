using TaskManager.DataLayer.Entities;
using System.Linq;
    
namespace TaskManager.DataLayer.Repositories
{
    public interface IQueryableRepository<TDbEntity> where TDbEntity : IDbEntity
    {
        IQueryable<TDbEntity> Find();
    }
}