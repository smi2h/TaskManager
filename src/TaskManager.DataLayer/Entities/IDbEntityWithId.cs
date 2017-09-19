namespace TaskManager.DataLayer.Entities
{
    /// <summary>
    /// Сущность базы с числовым ПК
    /// </summary>
    public interface IDbEntityWithId : IDbEntity
    {
        int Id { get; set; }
    }
}