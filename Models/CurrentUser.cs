using System.Text.Json.Serialization;

namespace AppwriteWithBlazor.Models
{
    public class CurrentUser
    {
        [JsonPropertyNameAttribute("$id")]
        public string Id { get; set; }

        [JsonPropertyNameAttribute("userId")]
        public string UserId { get; set; }

        [JsonPropertyNameAttribute("providerUid")]
        public string Email { get; set; }
    }
}
