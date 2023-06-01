namespace AppwriteWithBlazor.Models
{
    public class DocumentCreate<T>
    {
        public string DocumentId { get; set; }
        public T Data { get; set; }
        //public Dictionary<string, dynamic> Read { get; set; }
        //public Dictionary<string, dynamic> Write { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}
