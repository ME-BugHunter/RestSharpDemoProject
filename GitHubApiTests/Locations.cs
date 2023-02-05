using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GitHubApiTests
{
    internal class Locations
    {
        [JsonPropertyName("post code")]
        public string postCode { get; set; }

        [JsonPropertyName("country abbreviation")]
        public string countryAbbrev { get; set; }
        public string country { get; set; } 
    }
}