using System;
using System.ComponentModel.DataAnnotations;

namespace AccountService.Models
{
    public class CreateAccountModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}