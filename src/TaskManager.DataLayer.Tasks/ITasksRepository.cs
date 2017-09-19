using System;
using System.Threading.Tasks;
using TaskManager.Domain.Tasks.Models;
using TaskManager.RegistryCommon;

namespace TaskManager.DataLayer.Tasks
{
    public interface ITasksRepository
    {
        Task AddAsync(TaskDomain toAdd);

        Task<TaskDomain> GetAsync(TaskRef id);

        Task DeleteAsync(TaskRef id);

        Task UpdateAsync(TaskDomain task);

        Task<TableResponseCommon<TaskDomain>> SelectTasksAsync(TaskSearchParams searchParams);
    }
}
