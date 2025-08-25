using System.Text.Json.Serialization;

namespace Tujyane.Models
{
    public class AppwriteUser
    {
        [JsonPropertyName("$id")]
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [JsonIgnore] // do not serialize to JSON
        public string[] NameSegments
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return new string[0];

                return Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            }
        }
        [JsonIgnore]
        public string FirstName => NameSegments.Length > 0 ? NameSegments[0] : string.Empty;

        [JsonIgnore]
        public string LastName => NameSegments.Length > 1 ? NameSegments[NameSegments.Length - 1] : string.Empty;
    }
}
