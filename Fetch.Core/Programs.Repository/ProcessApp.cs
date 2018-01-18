using System.Diagnostics;

namespace Programs.Repository
{
    public class ProcessApp
    {
        public ProcessApp(string processName)
        {
            ProcessName = processName;
        }
        public ProcessApp(Process process)
        {
            ProcessName = process.ProcessName;
        }
        public string ProcessName { get; set; }
    }
}