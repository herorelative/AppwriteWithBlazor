using System.ComponentModel.DataAnnotations;

namespace AppwriteWithBlazor.Models
{
    public class TodoCreateModel
    {
        [Required]
        [MaxLength(50)]
        public string Content { get; set; }

        public bool IsDone { get; set; }

        [MaxLength(36)]
        public string ProjectId { get; set; }
    }
}
