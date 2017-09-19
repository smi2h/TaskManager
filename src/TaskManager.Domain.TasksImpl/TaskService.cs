using System;
using System.Threading.Tasks;
using TaskManager.DataLayer.Tasks;
using TaskManager.DataLayer.TransactionManagement;
using TaskManager.Domain.Tasks.Models;
using TaskManager.Domain.TasksImpl;
using TaskManager.RegistryCommon;

namespace TaskManager.Domain.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskStateMachine _stateMachine;

        public TaskService(ITasksRepository tasksRepository, IUnitOfWork unitOfWork, ITaskStateMachine stateMachine)
        {
            _tasksRepository = tasksRepository;
            _unitOfWork = unitOfWork;
            _stateMachine = stateMachine;
        }

        public async Task AddAsync(TaskDomain toAdd)
        {
            await _tasksRepository.AddAsync(toAdd).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task<TaskDomain> GetAsync(TaskRef id)
        {
            return _tasksRepository.GetAsync(id);
        }

        public async Task DeleteAsync(TaskRef id)
        {
            await _tasksRepository.DeleteAsync(id).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task<TableResponseCommon<TaskDomain>> SelectTasksAsync(TaskSearchParams searchParams)
        {
            return _tasksRepository.SelectTasksAsync(searchParams);
        }

        public async Task<TaskDomain> ReScheduleAsync(TaskRef id, DateTime newDate)
        {
            var task = await _tasksRepository.GetAsync(id);
            task.DueDate = newDate;

            await _tasksRepository.UpdateAsync(task).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return task;
        }

        public async Task<TaskDomain> CancelAsync(TaskRef id)
        {
            var task = await _tasksRepository.GetAsync(id);
            _stateMachine.ValidateSwitch(task.State, TaskState.Canceled);
            task.State = TaskState.Canceled;

            await _tasksRepository.UpdateAsync(task).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return task;
        }
    }
}
