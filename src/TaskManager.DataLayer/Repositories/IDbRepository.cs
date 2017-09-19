using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.DataLayer.Entities;

namespace TaskManager.DataLayer.Repositories
{
    /// <summary>
    /// Репозиторий с int Id в качестве PK
    /// </summary>
    public interface IDbRepository<TDbEntity> : IQueryableRepository<TDbEntity> where TDbEntity : IDbEntityWithId
    {
        TDbEntity FindById(int id);

        Task<TDbEntity> FindByIdAsync(int id);

        TDbEntity GetById(int id);

        Task<TDbEntity> GetByIdAsync(int id);

        void Add(TDbEntity dbEntity);

        void AddMany(IEnumerable<TDbEntity> dbEntities);

        void Delete(TDbEntity dbEntity);

        void DeleteMany(IEnumerable<TDbEntity> dbEntities);
    }
}