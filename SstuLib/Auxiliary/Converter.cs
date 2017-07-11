using Newtonsoft.Json;

namespace SstuLib.Auxiliary
{
    public static class Converter
    {
        public static string Convert(Request req)
        {
            var jsonSettings = new JsonSerializerSettings{
                DateFormatString = "yyyy-MM-dd",
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(req, jsonSettings);
        }
    }
}