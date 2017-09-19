using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TaskManager.RegistryCommon
{
    public static class PagerableOrderableHelper
    {
        private static readonly MethodInfo SkipMethodInfo = typeof(System.Linq.Queryable).GetTypeInfo().GetDeclaredMethod("Skip");
        private static readonly MethodInfo TakeMethodInfo = typeof(System.Linq.Queryable).GetTypeInfo().GetDeclaredMethod("Take");

        public static IQueryable<T> Page<T>(this IOrderedQueryable<T> obj, int page, int pageSize, out int objectsCount)
        {
            objectsCount = System.Linq.Queryable.Count<T>(obj);
            if (page > 1 && (page - 1) * pageSize >= objectsCount)
                page = (objectsCount - 1) / pageSize + 1;
            return PagerableOrderableHelper.Page<T>(obj, page, pageSize);
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> obj, int page, int pageSize, out int objectsCount)
        {
            objectsCount = System.Linq.Queryable.Count<T>(obj);
            return obj.Page<T>(page, pageSize);
        }

        public static IQueryable<T> Page<T>(this IOrderedQueryable<T> obj, int page, int pageSize)
        {
            if (page < 1)
                page = 1;
            int count = (page - 1) * pageSize;
            return System.Linq.Queryable.Take<T>(System.Linq.Queryable.Skip<T>(obj, count), pageSize);
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> obj, int page, int pageSize)
        {
            if (page < 1)
                page = 1;
            int count = (page - 1) * pageSize;
            return System.Linq.Queryable.Take<T>(System.Linq.Queryable.Skip<T>(obj, count), pageSize);
        }

        public static IQueryable<T> Page<T, TResult>(this IQueryable<T> obj, int page, int pageSize, Expression<Func<T, TResult>> keySelector, bool asc)
        {
            if (page < 1)
                page = 1;
            int count = (page - 1) * pageSize;
            return asc ? System.Linq.Queryable.Take<T>(System.Linq.Queryable.Skip<T>(obj.OrderBy<T, TResult>(keySelector), count), pageSize) : System.Linq.Queryable.Take<T>(System.Linq.Queryable.Skip<T>(obj.OrderByDescending<T, TResult>(keySelector), count), pageSize);
        }

        public static IQueryable<T> Page<T, TResult>(this IQueryable<T> obj, int page, int pageSize, Expression<Func<T, TResult>> keySelector, bool asc, out int objectsCount)
        {
            objectsCount = System.Linq.Queryable.Count<T>(obj);
            if (page < 1)
                page = 1;
            int count = (page - 1) * pageSize;
            return asc ? System.Linq.Queryable.Take<T>(System.Linq.Queryable.Skip<T>(obj.OrderBy<T, TResult>(keySelector), count), pageSize) : System.Linq.Queryable.Take<T>(System.Linq.Queryable.Skip<T>(obj.OrderByDescending<T, TResult>(keySelector), count), pageSize);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return PagerableOrderableHelper.OrderingHelper<T>(source, propertyName, false, false);
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return PagerableOrderableHelper.OrderingHelper<T>(source, propertyName, true, false);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool asc)
        {
            return PagerableOrderableHelper.OrderingHelper<T>(source, propertyName, !asc, false);
        }

        public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> expr, bool asc)
        {
            return asc ? source.OrderBy<T, TKey>(expr) : source.OrderByDescending<T, TKey>(expr);
        }

        public static IOrderedQueryable<T> ThenBy<T, TKey>(this IOrderedQueryable<T> source, Expression<Func<T, TKey>> expr, bool asc)
        {
            return asc ? source.ThenBy<T, TKey>(expr) : source.ThenByDescending<T, TKey>(expr);
        }

        public static IOrderedQueryable<T> OrderThenByAsc<T, TKey>(this IOrderedQueryable<T> source, Expression<Func<T, TKey>> expr, bool asc)
        {
            return asc ? source.ThenBy<T, TKey>(expr) : source.ThenByDescending<T, TKey>(expr);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return PagerableOrderableHelper.OrderingHelper<T>((IQueryable<T>)source, propertyName, false, true);
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return PagerableOrderableHelper.OrderingHelper<T>((IQueryable<T>)source, propertyName, true, true);
        }

        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), string.Empty);
            MemberExpression memberExpression = Expression.PropertyOrField((Expression)parameterExpression, propertyName);
            LambdaExpression lambdaExpression = Expression.Lambda((Expression)memberExpression, new ParameterExpression[1]
            {
        parameterExpression
            });
            MethodCallExpression methodCallExpression = Expression.Call(typeof(System.Linq.Queryable), (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty), new Type[2]
            {
        typeof (T),
        memberExpression.Type
            }, new Expression[2]
            {
        source.Expression,
        (Expression) Expression.Quote((Expression) lambdaExpression)
            });
            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>((Expression)methodCallExpression);
        }

        public static IQueryable<TSource> Skip<TSource>(this IQueryable<TSource> source, Expression<Func<int>> countAccessor)
        {
            return PagerableOrderableHelper.Parameterize<TSource, int>(PagerableOrderableHelper.SkipMethodInfo, source, countAccessor);
        }

        public static IQueryable<TSource> Take<TSource>(this IQueryable<TSource> source, Expression<Func<int>> countAccessor)
        {
            return PagerableOrderableHelper.Parameterize<TSource, int>(PagerableOrderableHelper.TakeMethodInfo, source, countAccessor);
        }

        private static IQueryable<TSource> Parameterize<TSource, TParameter>(MethodInfo methodInfo, IQueryable<TSource> source, Expression<Func<TParameter>> parameterAccessor)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (parameterAccessor == null)
                throw new ArgumentNullException(nameof(parameterAccessor));
            return source.Provider.CreateQuery<TSource>((Expression)Expression.Call((Expression)null, methodInfo.MakeGenericMethod(typeof(TSource)), new Expression[2]
            {
        source.Expression,
        parameterAccessor.Body
            }));
        }
    }
}
