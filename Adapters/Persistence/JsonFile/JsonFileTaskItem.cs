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
                Status = ConvertStatus(item.Status)
            };

            return taskItem;
        }

        public static explicit operator JsonFileTaskItem?(TaskItem? item)
        {
            if (item is null)
            {
                return null;
            }

            var taskItem = new JsonFileTaskItem
            {
                Id = item.Id,
                Description = item.Description,
                Status = ConvertStatus(item.Status)
            };

            return taskItem;
        }

        private static Status ConvertStatus(string status)
        {
            switch (status)
            {
                case "todo":
                    return ToDo;
                case "in-progress":
                    return InProgress;
                case "done":
                    return Done;
                default:
                    throw new Exception("Corrupted persistence file");
            }
        }

        private static string ConvertStatus(Status status)
        {
            switch (status)
            {
                case ToDo:
                    return "todo";
                case InProgress:
                    return "in-progress";
                case Done:
                    return "done";
                default:
                    throw new Exception("Invalid Status");
            }
        }
    }
}
