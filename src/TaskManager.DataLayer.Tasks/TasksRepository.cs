using System.Linq;
using System.Threading.Tasks;
using TaskManager.DataLayer.Repositories;
using TaskManager.DataLayer.Tasks.Entities;
using TaskManager.Domain.Tasks.Models;
using TaskManager.RegistryCommon;
using TaskManager.Universal.DateTime.DateTimeProvider;

namespace TaskManager.DataLayer.Tasks
{
    public class TasksRepository : ITasksRepository
    {
        private readonly IGuidFactory _guidFactory;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IDbTmRepository<DbTask> _taskRepository;

        public TasksRepository(
            IGuidFactory guidFactory,
            IDateTimeProvider dateTimeProvider,
            IDbTmRepository<DbTask> taskRepositoryRepository)
        {
            _guidFactory = guidFactory;
            _dateTimeProvider = dateTimeProvider;
            _taskRepository = taskRepositoryRepository;
        }

        public Task AddAsync(TaskDomain toAdd)
        {
            var dbTask = CreateDbTask(toAdd);
            _taskRepository.Add(dbTask);
            return Task.CompletedTask;
        }

        public async Task<TaskDomain> GetAsync(TaskRef id)
        {
            var dbTask = await _taskRepository.GetByIdAsync(id).ConfigureAwait(false);
            return ConvertTaskDomain(dbTask);
        }

        public async Task DeleteAsync(TaskRef id)
        {
            var dbTask = await _taskRepository.GetByIdAsync(id).ConfigureAwait(false);
            _taskRepository.Delete(dbTask); 
        }

        public async Task UpdateAsync(TaskDomain task)
        {
            var dbTask = await _taskRepository.GetByIdAsync(task.Id).ConfigureAwait(false);
            MapTaskToDb(dbTask, task);
        }

        public async Task<TableResponseCommon<TaskDomain>> SelectTasksAsync(TaskSearchParams searchParams)
        {
            var query = _taskRepository
                .Find()
                .Where(x => searchParams.State == null || x.State == searchParams.State)
                .Select(x=> new TaskDomain
                {
                    Priority = x.Priority,
                    State = x.State,
                    AddedDate = x.AddedDate,
                    DueDate = x.DueDate,
                    Description = x.Description,
                    Id = x.Id
                });

            return await query.ToTableResponseAsync(searchParams).ConfigureAwait(false);
        }
        
        private void MapTaskToDb(DbTask dbTask, TaskDomain taskDomain)
        {
            dbTask.State = taskDomain.State;
            dbTask.DueDate = taskDomain.DueDate;
            dbTask.Priority = taskDomain.Priority;
            dbTask.Description = taskDomain.Description; 
        }

        private DbTask CreateDbTask(TaskDomain task)
        {
            var dbTask = new DbTask
            {
                Id = _guidFactory.Create(),
                AddedDate = _dateTimeProvider.Now
            };

            MapTaskToDb(dbTask, task);
            return dbTask;
        }

        private TaskDomain ConvertTaskDomain(DbTask dbTask)
        {
            return new TaskDomain
            {
                Priority = dbTask.Priority,
                State = dbTask.State,
                AddedDate = dbTask.AddedDate,
                DueDate = dbTask.DueDate,
                Description = dbTask.Description,
                Id = dbTask.Id
            };
        }
    }
}