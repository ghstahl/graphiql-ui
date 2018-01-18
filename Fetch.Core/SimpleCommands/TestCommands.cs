using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Command.Common;
using Newtonsoft.Json;
using Synoptic;

namespace SimpleCommands
{
    public class HelloThereQuery
    {
        public string Name { get; set; }
    }

    [Command(RouteBase = "v1/test")]
    public class TestCommands
    {
        
        [CommandAction(Route = "app-domain", Method = "GET")]
        public async Task<string> GetAppDomain()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        [CommandAction(Route = "current-time", Method = "GET")]
        public async Task<dynamic> GetCurrentTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        [CommandAction(Route = "return-false", Method = "GET")]
        public async Task<bool> ReturnFalse()
        {
            return false;
        }
        [CommandAction(Route = "return-true", Method = "GET")]
        public async Task<bool> ReturnTrue()
        {
            return true;
        }
        [CommandAction(Route = "hello-there", Method = "GET")]
        public async Task<string> GetIsInstalled([CommandParameter(FromBody = true)]HelloThereQuery body)
        {
            return $".NET Core welcomes {body.Name}";
        }

    }
}
