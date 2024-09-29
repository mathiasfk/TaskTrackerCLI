namespace Core.Models
{
    public class TaskItem
    {
        public required int Id { get; set; }
        public required string Description { get; set; }
        public required Status Status { get; set; }
    }
}
