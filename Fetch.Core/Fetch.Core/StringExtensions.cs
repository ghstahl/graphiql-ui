namespace Fetch.Core
{
    public static class StringExtensions
    {
        public static string RemoveFirst(this string source, string remove)
        {
            int index = source.IndexOf(remove);
            return (index < 0)
                ? source
                : source.Remove(index, remove.Length);
        }
    }
}