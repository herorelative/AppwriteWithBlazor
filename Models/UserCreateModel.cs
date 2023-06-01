using System.ComponentModel.DataAnnotations;

namespace AppwriteWithBlazor.Models
{
    public class UserCreateModel
    {
        [MaxLength(36)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(36)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        public string Password { get; set; }

        [MaxLength(128)]
        public string? Name { get; set; }
    }
}
