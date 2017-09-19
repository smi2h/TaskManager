using System;

namespace TaskManager.Domain.Tasks.Models
{

    public class TaskRef : GuidRef
    {
        [Obsolete("Явно нигде не вызывать! Нужен для сериализатора", true)]
        public TaskRef()
        { }

        public TaskRef(Guid @ref) : base(@ref)
        { }

        public static implicit operator Guid(TaskRef taskRef)
        {
            return taskRef.Value;
        }

        public static implicit operator TaskRef(Guid @ref)
        {
            return new TaskRef(@ref);
        }
    }
}
