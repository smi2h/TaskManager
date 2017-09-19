using TaskManager.DataLayer.Entities;
 
namespace TaskManager.DataLayer.Repositories
{
    public interface IRepositoryFactory
    {
        IDbRepository<TDbEntity> GetRepository<TDbEntity>() where TDbEntity : class, IDbEntityWithId;

        IQueryableRepository<TDbEntity> GetQueriableRepository<TDbEntity>() where TDbEntity : class, IDbEntity;
    }
}