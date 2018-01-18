using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Synoptic;

namespace CommandFileLoader
{
    public class ReadFileQuery
    {
        public string Path { get; set; }
    }
    public class ConfigOptions
    {
        public string RootFolder { get; set; }
    }
    [Command(RouteBase = "v1/json-file")]
    public class JsonFile
    {
        public static string RootFolder { get; set; }

        [CommandAction(Route = "config", Method = "POST")]
        public async Task PostConfigAsync([CommandParameter(FromBody = true)]ConfigOptions body)
        {
            RootFolder = body.RootFolder;
            //  return new PrimitiveValue<bool>(true);
        }

        [CommandAction(Route = "read", Method = "GET")]
        public async Task<ExpandoObject> GetReadAsync([CommandParameter(FromBody = true)]ReadFileQuery body)
        {
            var path = Path.Combine(RootFolder, body.Path);
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                ExpandoObject eo = (ExpandoObject)serializer.Deserialize(file, typeof(ExpandoObject));
                return eo;
            }
        }
    }
}
