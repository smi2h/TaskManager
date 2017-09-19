using TaskManager.Domain.Tasks.Models;

namespace TaskManager.Domain.TasksImpl
{
    public interface ITaskStateMachine
    {
        void ValidateSwitch(TaskState current, TaskState requested);
    }
}
