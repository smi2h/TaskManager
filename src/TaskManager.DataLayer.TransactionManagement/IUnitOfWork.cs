using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DataLayer.Repositories;

namespace TaskManager.DataLayer.TransactionManagement
{
    public interface IUnitOfWork : IRepositoryFactory
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
