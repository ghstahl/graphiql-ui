using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Synoptic.CommandAction.Tests
{
    public class FetchInit
    {
        public string Method { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public dynamic Body { get; set; }
    }

    public class ComplexData
    {
        public string SomeString { get; set; }
        public int SomeInt { get; set; }
    }

    [Command(RouteBase = "My/Route/Base")]
    internal class CommandRunnerTestClass
    {
        public volatile static Action<MethodBase, object[]> TestAction = (m, a) => { };

        [CommandAction]
        public void TestCommand(string param1)
        {
            Dump(param1);
        }

        [CommandAction]
        public void CommandWithBool(bool param1)
        {
            Dump(param1);
        }

        [CommandAction]
        public void MultipleParamsToHyphen(string paramOne, string paramTwo, string paramThree)
        {
            Dump(paramOne, paramTwo, paramThree);
        }
        [CommandAction]
        public ComplexData MultipleParamsToHyphenWithReturn(string paramOne, string paramTwo, string paramThree)
        {
            Dump(paramOne, paramTwo, paramThree);
            return new ComplexData()
            {
                SomeInt = 42,
                SomeString = "Hello"
            };
        }

        [CommandAction(Route = "v1/complex", Method = "get")]
        public ComplexData ComplexParamToHyphenWithReturn([CommandParameter(FromBody = true)] ComplexData body)
        {
            Dump(body);
            return new ComplexData()
            {
                SomeInt = 42,
                SomeString = "Hello"
            };
        }

        private void Dump(params object[] args)
        {
            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();

            if (frames == null)
            {
                Assert.Fail("Method was not called.");
            }
            else
            {
                MethodBase caller = frames.Skip(1).First().GetMethod();
                Console.WriteLine();
                Console.WriteLine("Command: {0}", caller.Name);
                int i = 0;
                foreach (var parameterInfo in caller.GetParameters())
                    Console.WriteLine("  {0}={1}", parameterInfo.Name, args[i++]);

                TestAction(caller, args);
                return;
            }
        }
    }
    [TestClass]
    public class ComplexTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var camelSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var fetchInit = new FetchInit() { Headers = new Dictionary<string, string>(), Method = "Get" };

            var json = JsonConvert.SerializeObject(

                new ComplexData() { SomeInt = 42, SomeString = "Hello Cat" },
                camelSettings);
            fetchInit.Body = JObject.Parse(json);
            var jsonFetchInit = JsonConvert.SerializeObject(fetchInit, camelSettings);

            var fetchInit2 = JsonConvert.DeserializeObject<FetchInit>(jsonFetchInit);
            var jsonFetchInit2 = JsonConvert.SerializeObject(fetchInit2, camelSettings);

            var body = fetchInit2.Body;
            var jsonBody = JsonConvert.SerializeObject((object)body, camelSettings);


            var runResult = new CommandRunner().Run(new[]
            {
                "command-runner-test-class",
                "complex-param-to-hyphen-with-return",
                string.Format(@"--param-one={0}", json)
            });
            var routeQuery = new RouteQuery()
            {
                Body = new ComplexData() { SomeInt = 42, SomeString = "Hello Cat" },
                Method = "GET",
                Route = "My/Route/Base/v1/compleX"
            };
            runResult = await (new CommandRunner()).RunViaRouteAsync(routeQuery);

            runResult = new CommandRunner().WithCommandFromType<CommandRunnerTestClass>().Run(new[]
            {
                "command-runner-test-class",
                "multiple-params-to-hyphen",
                "--param-one=one",
                "--param-two=two",
                "--param-three=three"
            });
            runResult = new CommandRunner().WithCommandFromType<CommandRunnerTestClass>().Run(new[]
            {
                "command-runner-test-class",
                "multiple-params-to-hyphen-with-return",
                "--param-one=one",
                "--param-two=two",
                "--param-three=three"
            });
        }
    }
}
