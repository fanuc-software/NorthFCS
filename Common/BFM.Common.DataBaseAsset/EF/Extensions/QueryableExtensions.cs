using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace BFM.Common.DataBaseAsset
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName)
        {
            return QueryableHelper<T>.OrderBy(queryable, propertyName, false);
        }
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName, bool isAsc)
        {
            return QueryableHelper<T>.OrderBy(queryable, propertyName, isAsc);
        }
        static class QueryableHelper<T>
        {
            private static Dictionary<string, LambdaExpression> cache = new Dictionary<string, LambdaExpression>();
            public static IQueryable<T> OrderBy(IQueryable<T> queryable, string propertyName, bool isAsc)
            {
                string cacheName = string.Format("{0}-{1}", typeof(T).Name, propertyName);
                dynamic keySelector = GetLambdaExpression(cacheName, propertyName);
                return isAsc ? Queryable.OrderBy(queryable, keySelector) : Queryable.OrderByDescending(queryable, keySelector);
            }
            private static LambdaExpression GetLambdaExpression(string cacheName, string propertyName)
            {
                if (cache.ContainsKey(cacheName)) return cache[cacheName];
                var param = Expression.Parameter(typeof(T));
                var body = Expression.Property(param, propertyName);
                var keySelector = Expression.Lambda(body, param);
                cache[cacheName] = keySelector;
                return keySelector;
            }
        }
    }
}
