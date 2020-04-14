using System.ComponentModel.DataAnnotations;

namespace AccountService.Models
{
    public class DeleteModel
    {
        [Required]
        public string Email { get; set; }
    }
}