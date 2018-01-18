using Command.Common;
using Programs.Models;
using Programs.Repository;
using Synoptic;

namespace ProgramsCommand
{
    [Command(RouteBase = "v1/processes")]
    public class Processes
    {
        public static IProgramsRepository ProgramsRepository { get; set; }
        [CommandAction(Route = "load", Method = "POST")]
        public void PostLoad()
        {
            ProgramsRepository.LoadProcesses(false);
        }

        [CommandAction(Route = "count", Method = "GET")]
        public PrimitiveValue<int> GetCount()
        {
            var result = ProgramsRepository.ProcessCount;
            return new PrimitiveValue<int>(result);
        }

        [CommandAction(Route = "is-running", Method = "GET")]
        public PrimitiveValue<bool> GetIsRunning([CommandParameter(FromBody = true)]IsRunningQuery body)
        {
            var result = ProgramsRepository.IsRunning(body.ProcessName);
            return new PrimitiveValue<bool>(result);
        }

        [CommandAction(Route = "page", Method = "GET")]
        public ProcessApp[] GetPage([CommandParameter(FromBody = true)]PageQuery body)
        {
            var result = ProgramsRepository.PageProcess(body.Offset, body.Count);
            return result;
        }
    }
}