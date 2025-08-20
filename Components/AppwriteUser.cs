using System.Text.Json.Serialization;

namespace Tujyane.Models
{
    public class AppwriteUser
    {
        [JsonPropertyName("$id")]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
