using System.ComponentModel.DataAnnotations;

namespace AccountService.Models
{
    public class DeleteAccountModel
    {
        [Required]
        public string Email { get; set; }
    }
}