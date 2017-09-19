using TaskManager.RegistryCommon;

namespace TaskManager.Domain.Tasks.Models
{
    public class TaskSearchParams : IPagingParams
    { 
        public TaskState? State { get; set; }
        public PagingParams PagingParams { get; set; }
    }
}
