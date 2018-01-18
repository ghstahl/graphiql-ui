using System.Reflection;

namespace CommandGraphQL
{
    public static class Anchor
    {
        public static string FullName => Assembly.GetAssembly(typeof(Anchor)).FullName;
    }
}