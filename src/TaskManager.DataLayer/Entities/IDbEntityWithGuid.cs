using System;

namespace TaskManager.DataLayer.Entities
{
    /// <summary>
    /// Сущность с Guid PK
    /// </summary>
    public interface IDbEntityWithGuid : IDbEntity
    {
        Guid Id { get; set; }
    }
}