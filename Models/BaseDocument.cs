using System.Text.Json.Serialization;

namespace AppwriteWithBlazor.Models
{
    public class BaseDocument
    {
        [JsonPropertyName("$id")]
        public string Id { get; set; }

        [JsonPropertyName("$createdAt")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("$updatedAt")]
        public string UpdatedAt { get; set; }
    }
}
