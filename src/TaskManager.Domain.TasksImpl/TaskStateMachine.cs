using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Domain.Tasks.Models;

namespace TaskManager.Domain.TasksImpl
{
    public class TaskStateMachine : ITaskStateMachine
    {
        #region states graph

        private readonly Dictionary<TaskState, TaskState[]> _statesGraph = new Dictionary<TaskState, TaskState[]>
        {
            {
                TaskState.Inprogress, new[]
                {
                    TaskState.Canceled,
                    TaskState.Done
                }
            },
            {
                TaskState.Canceled, new[]
                {
                    TaskState.Inprogress,
                }
            },
        };

        #endregion

        public void ValidateSwitch(TaskState current, TaskState requested)
        {
            TaskState[] availableStates;
            if (!_statesGraph.TryGetValue(current, out availableStates))
            {
                var message = $"Из состояния платежа {current} нет доступных переходов. Запрошен переход в состояние {requested}.";
                throw new InvalidOperationException(message);
            }

            if (!availableStates.Any(s => s == requested))
            {
                var message = $"Из состояния платежа {current} нет перехода в запрошенное состояние {requested}.";
                throw new InvalidOperationException(message);
            }
        }
    }
}