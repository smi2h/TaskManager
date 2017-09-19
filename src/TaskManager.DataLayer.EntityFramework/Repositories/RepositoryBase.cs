using Microsoft.EntityFrameworkCore;
using TaskManager.DataLayer.Entities;

namespace Xrm.Pcc.DataLayer.EntityFramework.Repositories
{
    public abstract class RepositoryBase<TDbEntity> where TDbEntity : class, IDbEntity
    {
        protected RepositoryBase(TmContext tmContext)
        {
            TmContext = tmContext;
        }

        protected TmContext TmContext { get; }

        protected DbSet<TDbEntity> EntitiesSet => TmContext.Set<TDbEntity>();
    }
}