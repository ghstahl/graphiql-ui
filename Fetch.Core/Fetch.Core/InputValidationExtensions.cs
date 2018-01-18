using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Fetch.Core
{
    public static class InputValidationExtensions
    {
        private static readonly string[] AllowedMethods = { "GET", "PUT", "POST", "DELETE", "HEAD" };

        public static void ValidateStartsWith(this string o, string value)
        {
            if (!o.StartsWith(value, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"StartsWith [{value}] failed on subject:[{o}]");
            }
        }
        public static void ValidateMethod(this string stringToCheck)
        {
            var temp = stringToCheck.ToUpper();
            var valid = AllowedMethods.Any(temp.Contains);

            if (!valid)
            {
                throw new Exception($"Method [{stringToCheck}] is not allowed");
            }
        }

        public static Input ToInput(this object input)
        {

            string jsonBody = "{}";
            string url = "not set";
            string method = "not set";
            string jsonHeaders = "{}";

            ExpandoObject expandoInput = input as ExpandoObject;
            var expandoDict = expandoInput as IDictionary<string, object>;

            url = expandoDict["url"] as string;
            method = expandoDict["method"] as string;
            ExpandoObject body = expandoDict["body"] as ExpandoObject;
            var expandoBodyDict = body as IDictionary<string, object>;
            var containsFunc = false;
            foreach (var item in expandoBodyDict)
            {
                var type = item.Value.GetType();
                if (type == typeof(Func<object, Task<object>>))
                {
                    containsFunc = true;
                    break;
                }
            }


            ExpandoObject headers = expandoDict["headers"] as ExpandoObject;
            if (headers != null)
            {
                jsonHeaders = headers.ToJson();
            }
            if (!containsFunc)
            {
                jsonBody = body.ToJson();
            }

            url.ValidateStartsWith("local://");
            url = url.RemoveFirst("local://");
            method.ValidateMethod();

            return new Input()
            {
                Method = method,
                Url = url,
                Body = body,
                JsonHeaders = jsonHeaders,
                Headers = headers,
                JsonBody = jsonBody,
                BodyContainsFunc = containsFunc
            };
        }
    }
}