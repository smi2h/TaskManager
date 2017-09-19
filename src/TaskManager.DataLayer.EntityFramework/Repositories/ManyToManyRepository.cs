using TaskManager.DataLayer.Entities;
using TaskManager.DataLayer.Repositories;

namespace Xrm.Pcc.DataLayer.EntityFramework.Repositories
{
    public class ManyToManyRepository<TDbEntity> : QueryableRepository<TDbEntity>, IManyToManyRepository<TDbEntity> where TDbEntity : class, IDbManyToManyEntity
    {
        public ManyToManyRepository(TmContext tmContext) : base(tmContext)
        {
        }
    }
}