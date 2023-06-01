using System.ComponentModel.DataAnnotations;

namespace AppwriteWithBlazor.Models
{
    public class ProjectCreateModel
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(256)]
        public string Description { get; set; }
    }
}
