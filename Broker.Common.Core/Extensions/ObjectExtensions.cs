namespace Broker.Common.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static void ThrowIfNull<T>(this T obj)
            where T : class?
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
        }
    }
}