namespace Exev
{
    public static class EnumerableExtensions
    {
        public static bool IsBalanced<TSource, TResult>(this IEnumerable<TSource> items,
            Func<TSource, TResult> selector, Func<TResult, TResult, bool> test, (TResult, TResult) kinds)
        {
            var i = 0;
            foreach (var item in items)
            {
                if (test(kinds.Item1, selector(item)))
                {
                    i++;
                    continue;
                }
                if (test(kinds.Item2, selector(item)) && (--i) < 0)
                {
                    return false;
                }
            }
            return i == 0;
        }
    }
}
