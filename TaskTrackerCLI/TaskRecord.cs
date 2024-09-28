using System.Text.Json.Serialization;

namespace TaskTrackerCLI
{
    internal class TaskRecord
    {
        [JsonPropertyName("id")]
        public required int Id { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("status")]
        public required string Status { get; set; }
    }
}
