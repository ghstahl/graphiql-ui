using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Fetch.Core
{
    public static class ExpandoObjectExtensions
    {
        public static ExpandoObject Clone(this ExpandoObject original)
        {
            var expandoObjectConverter = new ExpandoObjectConverter();
            var originalDoc = JsonConvert.SerializeObject(original, expandoObjectConverter);
            dynamic clone = JsonConvert.DeserializeObject<ExpandoObject>(originalDoc, expandoObjectConverter);
            return clone;
        }

        public static string ToJson(this ExpandoObject original)
        {
            var expandoObjectConverter = new ExpandoObjectConverter();
            var originalDoc = JsonConvert.SerializeObject(original, expandoObjectConverter);
            return originalDoc;
        }

        public static ExpandoObject ToExpandoObject<T>(this T original)
        {
            var jsonValue = JsonConvert.SerializeObject(

                original,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            var expandoValue = JsonConvert.DeserializeObject<ExpandoObject>(jsonValue);
            return expandoValue;
        }
    }
}