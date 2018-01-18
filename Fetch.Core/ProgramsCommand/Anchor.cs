using System.Reflection;

namespace ProgramsCommand
{
    public static class Anchor
    {
        public static string FullName => Assembly.GetAssembly(typeof(Anchor)).FullName;
    }
}