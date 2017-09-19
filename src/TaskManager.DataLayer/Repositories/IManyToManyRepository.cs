using TaskManager.DataLayer.Entities;

namespace TaskManager.DataLayer.Repositories
{
    public interface IManyToManyRepository<TDbEntity> : IQueryableRepository<TDbEntity> where TDbEntity : IDbManyToManyEntity
    {
    }
}