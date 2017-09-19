using System.Linq;
using TaskManager.DataLayer.Entities;
using TaskManager.DataLayer.Repositories;

namespace Xrm.Pcc.DataLayer.EntityFramework.Repositories
{
    public class QueryableRepository<TDbEntity> : RepositoryBase<TDbEntity>, IQueryableRepository<TDbEntity> where TDbEntity : class, IDbEntity
    {
        public QueryableRepository(TmContext tmContext) : base(tmContext)
        {
        }

        public IQueryable<TDbEntity> Find()
        {
            return EntitiesSet;
        }
    }
}