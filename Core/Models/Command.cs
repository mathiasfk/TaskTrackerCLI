namespace Core.Models
{
    public enum CommandVerb
    {
        Add,
        Update,
        Delete,
        List,
        Mark
    }

    public class Command
    {
        public required CommandVerb Verb { get; set; }
        public int? Id { get; set; }
        public string? Description { get; set; }
        public Status? Status { get; set; }
    }
}
