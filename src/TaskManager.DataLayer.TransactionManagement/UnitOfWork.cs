using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DataLayer.Entities;
using TaskManager.DataLayer.Repositories;
using Xrm.Pcc.DataLayer.EntityFramework;
using Xrm.Pcc.DataLayer.EntityFramework.Repositories;

namespace TaskManager.DataLayer.TransactionManagement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDictionary<Type, object> _repositoryCache = new Dictionary<Type, object>();
        public UnitOfWork(TmContext context)
        {
            _context = context;
        }

        public IDbRepository<TDbEntity> GetRepository<TDbEntity>() where TDbEntity : class, IDbEntityWithId
        {
            var type = typeof(TDbEntity);
            if (!_repositoryCache.ContainsKey(type))
            {
                _repositoryCache[type] = new DbRepository<TDbEntity>(_context);
            }
            return (DbRepository<TDbEntity>)_repositoryCache[type];
        }

        public IQueryableRepository<TDbEntity> GetQueriableRepository<TDbEntity>() where TDbEntity : class, IDbEntity
        {
            return new QueryableRepository<TDbEntity>(_context);
        }

        private readonly TmContext _context;



        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
