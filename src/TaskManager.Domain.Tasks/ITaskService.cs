using System;
using System.Threading.Tasks;
using TaskManager.Domain.Tasks.Models;
using TaskManager.RegistryCommon;

namespace TaskManager.Domain.Tasks
{
    public interface ITaskService
    {
        Task AddAsync(TaskDomain toAdd);

        Task<TaskDomain> GetAsync(TaskRef id);

        Task DeleteAsync(TaskRef id);

        Task<TableResponseCommon<TaskDomain>> SelectTasksAsync(TaskSearchParams searchParams);

        Task<TaskDomain> ReScheduleAsync(TaskRef id, DateTime newDate);

        Task<TaskDomain> CancelAsync(TaskRef id);
    }
}
