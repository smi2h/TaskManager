using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DataLayer.Entities;
using TaskManager.DataLayer.Repositories;

namespace Xrm.Pcc.DataLayer.EntityFramework.Repositories
{
    public class DbTmRepository<TDbEntity> : QueryableRepository<TDbEntity>, IDbTmRepository<TDbEntity> where TDbEntity : class, IDbEntityWithGuid
    {
        public DbTmRepository(TmContext tmContext) : base(tmContext)
        { }

        public async Task<TDbEntity> GetByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id).ConfigureAwait(false);
            ThrowIfNull(id, result);
            return result;
        }
        
        private static void ThrowIfNull(Guid id, TDbEntity result)
        {
            if (result == null)
            {
                throw new ArgumentException($"Запись {typeof(TDbEntity).FullName} с id {id} не найдена");
            }
        }

        public void Add(TDbEntity dbEntity)
        {
            EntitiesSet.Add(dbEntity);
        }

        public void AddMany(IEnumerable<TDbEntity> dbEntities)
        {
            EntitiesSet.AddRange(dbEntities);
        }

        public void Delete(TDbEntity dbEntity)
        {
            EntitiesSet.Remove(dbEntity);
        }

        public void DeleteMany(IEnumerable<TDbEntity> dbEntities)
        {
            var entities = dbEntities.ToArray();
            EntitiesSet.RemoveRange(entities);
        }

        public Task<TDbEntity> FindByIdAsync(Guid id)
        {
            return EntitiesSet.FindAsync(id);
        }

        public TDbEntity GetById(Guid id)
        {
            var result = FindById(id);
            ThrowIfNull(id, result);
            return result;
        }

        public TDbEntity FindById(Guid id)
        {
            return EntitiesSet.Find(id);
        }
    }
}