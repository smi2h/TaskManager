using System;
using System.Linq; 

namespace TaskManager.RegistryCommon
{
    public static class RegistryResponseCommonExt
    {
        public static TableResponseCommon<TOut> Project<TIn, TOut>(this TableResponseCommon<TIn> input, Func<TIn, TOut> convert)
        {
            return new TableResponseCommon<TOut>
            {
                ItemsCount = input.ItemsCount,
                Items = input.Items.Select(convert).ToArray()
            };
        }
    }
}