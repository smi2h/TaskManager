using System.Collections.Generic;

namespace TaskManager.RegistryCommon
{
    public class TableResponseCommon<TModel>
    {
        public IEnumerable<TModel> Items { get; set; }
        public int ItemsCount { get; set; }
    }
}
