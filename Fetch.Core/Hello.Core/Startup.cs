using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Some.Library;


namespace Hello.Core
{
    public class Startup
    {
       // public CommandCompositionHelper CommandCompositionHelper { get; set; }

        public Startup()
        {

        }

        public async Task<object> Invoke(object input)
        {
            throw new Exception("no magic entry points allowed!");
        }

        public async Task<object> TestMethod(object input)
        {
            // this works which make me think that Newtonsoft.Json.dll is loadable either by being somewhere on disk or 
            // because of the nature of the assembly.
            var dd = JsonConvert.SerializeObject("Hello from dot net core 2");
            var pp = new SomeClass().GetDateTimeNow();



            return dd + pp;
        }
    }
}
