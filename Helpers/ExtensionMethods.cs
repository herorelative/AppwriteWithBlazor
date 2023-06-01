using System.Text.Json;
using System.Text.Json.Serialization;

namespace AppwriteWithBlazor.Helpers
{
    public static class ExtensionMethods
    {
        public static JsonSerializerOptions DeserializerSettings { get; set; } = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

        public static JsonSerializerOptions SerializerSettings { get; set; } = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };
    }
}
