using Core.Models;
using System.Text.Json.Serialization;
using static Core.Models.Status;

namespace Adapters.Persistence.JsonFile
{
    internal class JsonFileTaskItem
    {
        [JsonPropertyName("id")]
        public required int Id { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("status")]
        public required string Status { get; set; }


        public static explicit operator TaskItem?(JsonFileTaskItem? item)
        {
            if (item is null)
            {
                return null;
            }

            var taskItem = new TaskItem
            {
                Id = item.Id,
                Description = item.Description,
                Status = ToDo
            };

            switch (item.Status)
            {
                case "todo":
                    taskItem.Status = ToDo; break;
                case "in-progress":
                    taskItem.Status = InProgress; break;
                case "done":
                    taskItem.Status = InProgress; break;
                default:
                    throw new Exception("corrupted persistence file");
            }

            return taskItem;
        }
    }
}
