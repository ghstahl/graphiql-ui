using System.Reflection;

namespace CommandFileLoader
{
    public static class Anchor
    {
        public static string FullName => Assembly.GetAssembly(typeof(Anchor)).FullName;
    }
}