namespace AppwriteWithBlazor.Models
{
    public class Project : BaseDocument
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class Todo : BaseDocument
    {
        public string Content { get; set; }
        public bool IsDone { get; set; }
        public string ProjectId { get; set; }
    }
}
