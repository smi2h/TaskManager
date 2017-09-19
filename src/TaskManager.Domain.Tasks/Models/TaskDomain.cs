using System;

namespace TaskManager.Domain.Tasks.Models
{
    public class TaskDomain 
    {
        public Guid Id { get; set; }
         
        public string Description { get; set; }
        
        public TaskPriority Priority { get; set; }
        
        public TaskState State { get; set; }
        
        public DateTime DueDate { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
