using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DataLayer.Entities;
using TaskManager.Domain.Tasks.Models;

namespace TaskManager.DataLayer.Tasks.Entities
{
    [Schema("tasks")]
    public class DbTask : IDbEntityWithGuid
    {
        /// <summary>
        /// Внутренний идентификатор акта в системе (PK)
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Номер акта
        /// </summary>
        [Required]
        [MaxLength(TypeConstants.ShortStringLength)]
        public string Description { get; set; }

        /// <summary>
        /// Приоритет
        /// </summary>
        public TaskPriority Priority { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public TaskState State { get; set; }

        /// <summary>
        /// Время добавления
        /// </summary> 
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// Срок
        /// </summary> 
        public DateTime DueDate  { get; set; }
    }
}
