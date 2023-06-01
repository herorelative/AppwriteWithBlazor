namespace AppwriteWithBlazor.Models
{
    public class Document<T>
    {
        public int Total { get; set; }
        public T Documents { get; set; }
    }
}
