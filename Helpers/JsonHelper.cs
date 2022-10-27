using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Dynamic;

namespace ZaloOA_v2.Helpers
{
    public class JsonHelper
    {
        public static string Serialize(object data, int DateFormatHandling = 0)
        {
            return JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings
            {
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat
            });
        }

        public static T Deserialize<T>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch
            {
                return default;
            }
        }

        public static dynamic Deserialize(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var converter = new ExpandoObjectConverter();
                dynamic tmp = JsonConvert.DeserializeObject<ExpandoObject>(data, converter);
                return tmp;
            }
            return null;
        }
    }
}
