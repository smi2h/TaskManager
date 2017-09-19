using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.DataLayer.Entities;

namespace TaskManager.DataLayer.Repositories
{
    public interface IDbTmRepository<TDbEntity> : IQueryableRepository<TDbEntity> where TDbEntity : IDbEntityWithGuid
    {
        TDbEntity FindById(Guid id);

        Task<TDbEntity> FindByIdAsync(Guid id);

        TDbEntity GetById(Guid id);

        Task<TDbEntity> GetByIdAsync(Guid id);

        void Add(TDbEntity dbEntity);

        void AddMany(IEnumerable<TDbEntity> dbEntities);

        void Delete(TDbEntity dbEntity);

        void DeleteMany(IEnumerable<TDbEntity> dbEntities);
    }
}