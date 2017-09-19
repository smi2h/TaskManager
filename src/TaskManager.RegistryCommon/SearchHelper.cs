using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.RegistryCommon
{
    public static class SearchHelper
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, PagingParams model, out int itemsTotalCount, Func<IQueryable<T>, IOrderedQueryable<T>> defaultSorting = null)
        {
            var orderBy = model.OrderBy;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                query = query.OrderBy(orderBy, model.OrderAsc);
            }
            else
            {
                if (defaultSorting == null)
                {
                    defaultSorting = x => x.OrderBy("Id");
                }
                query = defaultSorting(query);
            }
            var itemsPerPage = model.ItemsPerPage ?? ItemsPerPage;
            return query.Page(model.Page, itemsPerPage, out itemsTotalCount);
        }
        
        public static async Task<TableResponseCommon<T>> ToTableResponseAsync<T>(this IQueryable<T> query,IPagingParams searchModel)
        {
            int itemsCount;
            var items = await query.Paging(searchModel.PagingParams, out itemsCount).ToArrayAsync();
            return new TableResponseCommon<T>
            {
                Items = items,
                ItemsCount = itemsCount
            };
        }
        
        public const int ItemsPerPage = 20;
    }
}
