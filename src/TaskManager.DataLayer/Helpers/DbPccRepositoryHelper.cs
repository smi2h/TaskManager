using System;
using System.Threading.Tasks;
using TaskManager.DataLayer.Entities;
using TaskManager.DataLayer.Exceptions;
using TaskManager.DataLayer.Repositories;

namespace TaskManager.DataLayer.Helpers
{
    public static class DbPccRepositoryHelper
    {
        public static TDbEntity GetById<TDbEntity>(this IDbTmRepository<TDbEntity> repository, Guid id) where TDbEntity : IDbEntityWithGuid
        {
            var result = repository.FindById(id);
            AssertFound(id, result);
            return result;
        }

        public static async Task<TDbEntity> GetByIdAsync<TDbEntity>(this IDbTmRepository<TDbEntity> repository, Guid id) where TDbEntity : IDbEntityWithGuid
        {
            var result = await repository.FindByIdAsync(id);
            AssertFound(id, result);
            return result;
        }

        // ReSharper disable once UnusedParameter.Local
        private static void AssertFound<TDbEntity>(Guid id, TDbEntity result) where TDbEntity : IDbEntityWithGuid
        {
            if (result == null)
            {
                throw new DataLayerException($"Запись {typeof(TDbEntity)} с Id {id} не найдена");
            }
        }
    }
}
