using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Synoptic.Exceptions;
using Synoptic.Extensions;

namespace Synoptic
{
    public class RunResult
    {
        public Type ReturnType { get; set; }
        public object Value { get; set; }
        public int ErrorCode { get; set; }
    }
    public static class CommandActionExtensions
    {

        internal static async Task<RunResult> RunAsync(this CommandAction commandAction, ICommandDependencyResolver resolver,
            CommandLineParseResult parseResult)
        {
            var instance = resolver.Resolve(commandAction.LinkedToMethod.DeclaringType);
            object[] parameterValues = GetCommandParameterValues(commandAction.Parameters, parseResult);

            var returnResult = new RunResult() { ReturnType = commandAction.LinkedToMethod.ReturnType };
            if (commandAction.LinkedToMethod.ReturnType != typeof(void))
            {
                object res = null;
                if (commandAction.LinkedToMethod.IsAsyncMethod())
                {
                    dynamic result =  (Task) commandAction.LinkedToMethod.Invoke(instance, parameterValues);
                    await result;
                    
                    try
                    {
                        res = result.Result;
                    }
                    catch (Exception e)
                    {
                        res = null;
                    }

                }
                else
                {
                    res = commandAction.LinkedToMethod.Invoke(instance, parameterValues);
                }
                returnResult.Value = res;
                /*
                returnResult.Json = JsonConvert.SerializeObject(

                    res,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                    */
            }
            else
            {
                commandAction.LinkedToMethod.Invoke(instance, parameterValues);
            }
            return returnResult;
        }


        internal static RunResult Run(this CommandAction commandAction, ICommandDependencyResolver resolver,
            CommandLineParseResult parseResult)
        {
            var instance = resolver.Resolve(commandAction.LinkedToMethod.DeclaringType);
            object[] parameterValues = GetCommandParameterValues(commandAction.Parameters, parseResult);

            var returnResult = new RunResult() {ReturnType = commandAction.LinkedToMethod.ReturnType};
            if (commandAction.LinkedToMethod.ReturnType != typeof(void))
            {
                var res = commandAction.LinkedToMethod.Invoke(instance, parameterValues);
                returnResult.Value = res;
                /*
                returnResult.Json = JsonConvert.SerializeObject(

                    res,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                    */
            }
            else
            {
                commandAction.LinkedToMethod.Invoke(instance, parameterValues);
            }
            return returnResult;
        }

        private static object[] GetCommandParameterValues(IEnumerable<ParameterInfoWrapper> parameters, 
            CommandLineParseResult parseResult)
        {
            var args = new List<object>();
            foreach (var parameter in parameters)
            {
                var parameterName = parameter.Name;

                CommandLineParameter commandLineParameter =
                    parseResult.ParsedParameters.FirstOrDefault(p => p.Name.SimilarTo(parameterName));
                
                object value = null;
                
                // Method has parameter which was not supplied.
                if (commandLineParameter == null || commandLineParameter.Value == null)
                {
                    if (parameter.DefaultValue != null)
                    {
                        value = parameter.DefaultValue;
                    }
                    else if(parameter.IsRequired)
                    {
                        throw new CommandParameterInvalidException(String.Format("The parameter '{0}' is required.", parameter.Name));
                    }
                }
                else
                {
                    value = commandLineParameter.Value;
                }

                if (value != null)
                {
                    args.Add(GetConvertedParameterValue(parameter, value));
                    continue;
                }

                args.Add(null);
            }

            return args.ToArray();
        }

        private static object GetConvertedParameterValue(ParameterInfoWrapper parameter, object value)
        {
            if (!parameter.IsValueRequiredWhenOptionIsPresent)
                value = value != null;
            if (parameter.FromBody && value is string)
            {
                var data = JsonConvert.DeserializeObject(
                    (string)value,
                    parameter.Type
                );
                return data;
            }
            if (parameter.FromBody && value is ExpandoObject)
            {
                var expandoDict = value as IDictionary<string, object>;

                var data = expandoDict.ToObject(parameter.Type);
                return data;
            }
            return Convert.ChangeType(value, parameter.Type);
        }
    }
}