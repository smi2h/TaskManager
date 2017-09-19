using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DataLayer.Entities;
using TaskManager.DataLayer.Repositories;

namespace Xrm.Pcc.DataLayer.EntityFramework.Repositories
{
    public class DbRepository<TDbEntity> : QueryableRepository<TDbEntity>, IDbRepository<TDbEntity> where TDbEntity : class, IDbEntityWithId
    {
        public DbRepository(TmContext tmContext) : base(tmContext)
        { }

        public async Task<TDbEntity> GetByIdAsync(int id)
        {
            var result = await FindByIdAsync(id).ConfigureAwait(false);
            if (result == null)
            {
                //TODO: PCC-7 подумать над исключениями DAL
                throw new InvalidOperationException();
            }
            return result;
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

        public async Task<TDbEntity> FindByIdAsync(int id)
        {
            return await EntitiesSet.FindAsync(id).ConfigureAwait(false);
        }

        public TDbEntity GetById(int id)
        {
            var result = FindById(id);
            if (result == null)
            {
                //TODO: PCC-7 подумать над исключениями DAL
                throw new InvalidOperationException();
            }
            return result;
        }

        public TDbEntity FindById(int id)
        {
            return EntitiesSet.Find(id);
        }
    }
}