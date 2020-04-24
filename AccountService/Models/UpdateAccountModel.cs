using System.ComponentModel.DataAnnotations;

namespace AccountService.Models
{
    public class UpdateAccountModel
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isDAppOwner { get; set; }
        public bool isDelegate { get; set; }
    }
}